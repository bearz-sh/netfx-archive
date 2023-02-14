using System.Runtime.InteropServices;

using Bearz.Diagnostics;
using Bearz.Extra.Collections;
using Bearz.Extra.Strings;
using Bearz.Std;

using Path = System.IO.Path;

namespace Bearz.Cli.Execution;

public class Executable
{
    public Executable(ICliExecutionContext? context = null)
    {
        this.Context = null;
    }

    public string Name { get; set; } = string.Empty;

    public string? Location { get; set; }

    public IReadOnlyList<string> Windows { get; set; } = Array.Empty<string>();

    public IReadOnlyList<string> MacOs { get; set; } = Array.Empty<string>();

    public IReadOnlyList<string> Linux { get; set; } = Array.Empty<string>();

    internal static List<IPreCliHook> GlobalPreCliHooks { get; } = new();

    internal static List<IPostCliHook> GlobalPostCliHooks { get; } = new();

    internal List<IPreCliHook> PreCliHooks { get; } = new();

    internal List<IPostCliHook> PostCliHooks { get; } = new();

    protected internal ICliExecutionContext? Context { get; set; }

    public static void RegisterGlobalPreCliHook(IPreCliHook hook)
    {
        GlobalPreCliHooks.Add(hook);
    }

    public static void RegisterGlobalPostCliHook(IPostCliHook hook)
    {
        GlobalPostCliHooks.Add(hook);
    }

    public void RegisterPreCliHook(IPreCliHook hook)
    {
        this.PreCliHooks.Add(hook);
    }

    public void RegisterPostCliHook(IPreCliHook hook)
    {
        this.PreCliHooks.Add(hook);
    }

    public string? Which()
    {
        string? exe = this.Location;
        if (!exe.IsNullOrWhiteSpace())
            return exe;

        var envName = $"{this.Name.Replace("-", "_").ToUpperInvariant()}_EXE";

        if (this.Context is not null)
        {
            if (this.Context.Env.TryGet(envName, out exe) && this.Context.Fs.FileExists(exe))
            {
                this.Location = exe;
                return this.Location;
            }

            exe = this.Context.Process.Which(this.Name, null, true);
            if (exe is not null)
            {
                this.Location = exe;
                return exe;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                foreach (var path in this.Windows)
                {
                    exe = this.Context.Env.Expand(path);
                    if (!this.Context.Fs.FileExists(exe))
                    {
                        continue;
                    }

                    this.Location = exe;
                    return exe;
                }

                return null;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                foreach (var path in this.MacOs)
                {
                    exe = this.Context.Env.Expand(path);
                    if (!this.Context.Fs.FileExists(exe))
                    {
                        continue;
                    }

                    this.Location = exe;
                    return exe;
                }
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                foreach (var path in this.Linux)
                {
                    exe = this.Context.Env.Expand(path);
                    if (!this.Context.Fs.FileExists(exe))
                    {
                        continue;
                    }

                    this.Location = exe;
                    return exe;
                }
            }

            return null;
        }

        if (Env.TryGet(envName, out exe) && Fs.FileExists(exe))
        {
            this.Location = exe;
            return this.Location;
        }

        exe = Process.Which(this.Name, null, true);
        if (exe is not null)
        {
            this.Location = exe;
            return exe;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            foreach (var path in this.Windows)
            {
                exe = Env.Expand(path);
                if (!File.Exists(exe))
                {
                    continue;
                }

                this.Location = exe;
                return exe;
            }

            return null;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            foreach (var path in this.MacOs)
            {
                exe = Env.Expand(path);
                if (!File.Exists(exe))
                {
                    continue;
                }

                this.Location = exe;
                return exe;
            }
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            foreach (var path in this.Linux)
            {
                exe = Env.Expand(path);
                if (!File.Exists(exe))
                {
                    continue;
                }

                this.Location = exe;
                return exe;
            }
        }

        return null;
    }

    public virtual string WhichOrThrow()
    {
        var exe = this.Which();
        if (exe is null)
        {
            throw new NotFoundOnPathException(this.Name);
        }

        return exe;
    }

    public virtual Command CreateCommand(ICliCommand command)
    {
        var exe = this.WhichOrThrow();
        this.ApplyPreHooks(command);

        if (this.Context is null)
        {
            this.ApplyPreHooks(command);
            var si = command.Build();

            if (command.CliStartInfo.UseSudo && !RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                si.Args.Unshift(exe);
                exe = "sudo";
            }

            return Process.CreateCommand(exe, si);
        }
        else
        {
            if (command.CliStartInfo.Env is null)
            {
                command.CliStartInfo.Env = this.Context.Env.List();
            }
            else
            {
                var copy = this.Context.Env.List();
                foreach (var kvp in command.CliStartInfo.Env)
                {
                    copy[kvp.Key] = kvp.Value;
                }

                command.CliStartInfo.Env = copy;
            }

            command.CliStartInfo.Cwd ??= this.Context.Env.Cwd;

            var si = command.Build();

            if (!command.CliStartInfo.UseSudo || RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return this.Context.Process.CreateCommand(exe, si);
            }

            si.Args.Unshift(exe);
            exe = "sudo";

            return this.Context.Process.CreateCommand(exe, si);
        }
    }

    public CommandOutput Output(ICliCommandBuilder builder)
        => this.Output(builder.Build());

    public CommandOutput Output(ICliCommand command)
    {
        var cmd = this.CreateCommand(command);
        var o = cmd.Output();
        this.ApplyPostHooks(command, o);

        return o;
    }

    public Task<CommandOutput> OutputAsync(
        ICliCommandBuilder builder,
        CancellationToken cancellationToken = default)
    {
        return this.OutputAsync(builder.Build(), cancellationToken);
    }

    public async Task<CommandOutput> OutputAsync(
        ICliCommand command,
        CancellationToken cancellationToken = default)
    {
        var cmd = this.CreateCommand(command);
        var o = await cmd.OutputAsync(cancellationToken);
        this.ApplyPostHooks(command, o);

        return o;
    }

    protected virtual void ApplyPreHooks(ICliCommand command)
    {
        foreach (var hook in Executable.GlobalPreCliHooks)
        {
            hook.Next(this, command);
        }

        foreach (var hook in this.PreCliHooks)
        {
            hook.Next(this, command);
        }
    }

    protected virtual void ApplyPostHooks(ICliCommand command, CommandOutput output)
    {
        foreach (var hook in Executable.GlobalPostCliHooks)
        {
            hook.Next(this.Location!, command, output);
        }

        foreach (var hook in this.PostCliHooks)
        {
            hook.Next(this.Location!, command, output);
        }
    }
}
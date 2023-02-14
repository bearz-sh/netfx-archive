using System.Runtime.InteropServices;

using Bearz.Cli.Execution;
using Bearz.Diagnostics;
using Bearz.Extra.Collections;
using Bearz.Std;

namespace Bearz.Extensions.Cli;

public static class ExecutableInfoContextExtensions
{
    public static ExecutableInfo Clone(this ExecutableInfo executableInfo)
    {
        return new ExecutableInfo()
        {
            Name = executableInfo.Name,
            Location = executableInfo.Location,
            Linux = executableInfo.Linux,
            Windows = executableInfo.Windows,
            MacOs = executableInfo.MacOs,
        };
    }

    public static ExecutableInfoContext CloneWithContext(this ExecutableInfo executableInfo, ICliExecutionContext context)
    {
        return new ExecutableInfoContext(executableInfo, context);
    }

    public static Command CreateCommand(this ExecutableInfo executableInfo, ICliExecutionContext context, ICliCommand command)
    {
        executableInfo.ApplyPreHooks(command);

        var exe = executableInfo.FindOrThrow(context);

        if (command.CliStartInfo.Env is null)
        {
            command.CliStartInfo.Env = context.Env.List();
        }
        else
        {
            var copy = context.Env.List();
            foreach (var kvp in command.CliStartInfo.Env)
            {
                copy[kvp.Key] = kvp.Value;
            }

            command.CliStartInfo.Env = copy;
        }

        command.CliStartInfo.Cwd ??= context.Env.Cwd;

        var si = command.Build();

        if (!command.CliStartInfo.UseSudo || RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return Process.CreateCommand(exe, si);
        }

        si.Args.Unshift(exe);
        exe = "sudo";

        return Process.CreateCommand(exe, si);
    }

    public static CommandOutput Call(this ExecutableInfo executableInfo, ICliExecutionContext context, ICliCommand command)
    {
        var cmd = CreateCommand(executableInfo, context, command);
        var o = cmd.Output();
        executableInfo.ApplyPostHooks(command, o);

        return o;
    }

    public static async Task<CommandOutput> CallAsync(
        this ExecutableInfo executableInfo,
        ICliExecutionContext context,
        ICliCommand command,
        CancellationToken cancellationToken = default)
    {
        var cmd = CreateCommand(executableInfo, context, command);
        var o = await cmd.OutputAsync(cancellationToken);
        executableInfo.ApplyPostHooks(command, o);

        return o;
    }

    public static CommandOutput CallScript(this ExecutableInfo executableInfo, ICliExecutionContext context, CliScriptCommand command)
    {
        try
        {
            var cmd = CreateCommand(executableInfo, context, command);
            var output = cmd.Output();
            executableInfo.ApplyPostHooks(command, output);

            return output;
        }
        finally
        {
            if (File.Exists(command.TempFileName))
                File.Delete(command.TempFileName);
        }
    }

    public static async Task<CommandOutput> CallScriptAsync(this ExecutableInfo executableInfo, ICliExecutionContext context, CliScriptCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var cmd = CreateCommand(executableInfo, context, command);
            var output = await cmd.OutputAsync(cancellationToken)
                .ConfigureAwait(false);

            executableInfo.ApplyPostHooks(command, output);
            return output;
        }
        finally
        {
            if (File.Exists(command.TempFileName))
                File.Delete(command.TempFileName);
        }
    }

    public static string? Find(this ExecutableInfo info, ICliExecutionContext context)
    {
        if (info is null)
            throw new ArgumentNullException(nameof(info));

        if (context is null)
            throw new ArgumentNullException(nameof(context));

        if (!string.IsNullOrEmpty(info.Location))
            return info.Location;

        var exe = Process.Which(info.Name, null, true);
        if (exe is not null)
        {
            info.Location = exe;
            return exe;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            foreach (var path in info.Windows)
            {
                exe = Env.Expand(path);
                if (!File.Exists(exe))
                {
                    continue;
                }

                info.Location = exe;
                return exe;
            }

            return null;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            foreach (var path in info.MacOs)
            {
                exe = Env.Expand(path);
                if (!File.Exists(exe))
                {
                    continue;
                }

                info.Location = exe;
                return exe;
            }
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            foreach (var path in info.Linux)
            {
                exe = Env.Expand(path);
                if (!File.Exists(exe))
                {
                    continue;
                }

                info.Location = exe;
                return exe;
            }
        }

        return null;
    }

    public static string FindOrThrow(this ExecutableInfo info, ICliExecutionContext context)
    {
        var exe = Find(info, context);
        if (exe is null)
        {
            throw new NotFoundOnPathException(info.Name);
        }

        return exe;
    }
}
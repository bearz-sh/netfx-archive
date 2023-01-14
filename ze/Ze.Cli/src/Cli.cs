using System.Runtime.InteropServices;

using Bearz.Extra.Collections;
using Bearz.Std;

namespace Ze.Cli;

public static class Cli
{
    private static readonly List<IPreCliHook> PreCliHooks = new List<IPreCliHook>();

    private static readonly List<IPostCliHook> PostCliHooks = new List<IPostCliHook>();

    public static void RegisterPreCliHook(IPreCliHook hook)
    {
        PreCliHooks.Add(hook);
    }

    public static void RegisterPostCliHook(IPostCliHook hook)
    {
        PostCliHooks.Add(hook);
    }

    public static Command CreateCommand(ExecutableInfo executableInfo, CliCommand command)
    {
        foreach (var hook in PreCliHooks)
        {
            hook.Next(executableInfo, command);
        }

        var exe = executableInfo.FindOrThrow();
        var si = command.Build();

        if (command.CliStartInfo.UseSudo && !RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            si.Args.Unshift(exe);
            exe = "sudo";
        }

        return Process.CreateCommand(exe, si);
    }

    public static CommandOutput Call(ExecutableInfo executableInfo, CliCommand command)
    {
        var cmd = CreateCommand(executableInfo, command);
        var o = cmd.Output();
        foreach (var hook in PostCliHooks)
        {
            hook.Next(executableInfo.Location!, command, o);
        }

        return o;
    }

    public static async Task<CommandOutput> CallAsync(ExecutableInfo executableInfo, CliCommand command, CancellationToken cancellationToken = default)
    {
        var cmd = CreateCommand(executableInfo, command);
        var o = await cmd.OutputAsync(cancellationToken);
        foreach (var hook in PostCliHooks)
        {
            hook.Next(executableInfo.Location!, command, o);
        }

        return o;
    }
}
using System.Runtime.InteropServices;

using Bearz.Extra.Collections;
using Bearz.Std;

namespace Bearz.Cli.Execution;

public static class ExecutableInfoExtensions
{
    public static Command CreateCommand(this ExecutableInfo executableInfo, ICliCommand command)
    {
        foreach (var hook in ExecutableInfo.PreCliHooks)
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

    public static CommandOutput CallScript(ExecutableInfo executableInfo, CliScriptCommand command)
    {
        try
        {
            var cmd = CreateCommand(executableInfo, command);
            var output = cmd.Output();

            foreach (var hook in ExecutableInfo.PostCliHooks)
            {
                hook.Next(executableInfo.Location!, command, output);
            }

            return output;
        }
        finally
        {
            if (File.Exists(command.TempFileName))
                File.Delete(command.TempFileName);
        }
    }

    public static async Task<CommandOutput> CallScriptAsync(this ExecutableInfo executableInfo, CliScriptCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var cmd = CreateCommand(executableInfo, command);
            var output = await cmd.OutputAsync(cancellationToken)
                .ConfigureAwait(false);

            foreach (var hook in ExecutableInfo.PostCliHooks)
            {
                hook.Next(executableInfo.Location!, command, output);
            }

            return output;
        }
        finally
        {
            if (File.Exists(command.TempFileName))
                File.Delete(command.TempFileName);
        }
    }

    public static CommandOutput Call(this ExecutableInfo executableInfo, ICliCommand command)
    {
        var cmd = CreateCommand(executableInfo, command);
        var o = cmd.Output();
        foreach (var hook in ExecutableInfo.PostCliHooks)
        {
            hook.Next(executableInfo.Location!, command, o);
        }

        return o;
    }

    public static async Task<CommandOutput> CallAsync(this ExecutableInfo executableInfo, ICliCommand command, CancellationToken cancellationToken = default)
    {
        var cmd = CreateCommand(executableInfo, command);
        var o = await cmd.OutputAsync(cancellationToken);
        foreach (var hook in ExecutableInfo.PostCliHooks)
        {
            hook.Next(executableInfo.Location!, command, o);
        }

        return o;
    }

    public static void ApplyPreHooks(this ExecutableInfo executableInfo, ICliCommand command)
    {
        foreach (var hook in ExecutableInfo.PreCliHooks)
        {
            hook.Next(executableInfo, command);
        }
    }

    public static void ApplyPostHooks(this ExecutableInfo executableInfo, ICliCommand command, CommandOutput output)
    {
        foreach (var hook in ExecutableInfo.PostCliHooks)
        {
            hook.Next(executableInfo.Location!, command, output);
        }
    }
}
using System;
using System.Linq;

using Bearz.Std;

namespace Ze.Cli.Bash;

public static partial class Bash
{
    public static ExecutableInfo Executable { get; } = new ExecutableInfo()
    {
        Name = "bash",
        Windows = new[]
        {
            "%ProgramFiles%\\Git\\bin\\bash.exe",
            "%ProgramFiles%\\Git\\usr\\bin\\bash.exe",
            "%WINDIR%\\System32\\bash.exe",
        },
    };

    public static CommandOutput Call(Action<CliArgsCommandBuilder> configure)
    {
        var builder = new CliArgsCommandBuilder();
        configure(builder);
        return Call(builder.Build());
    }

    public static CommandOutput Call(CliArgsCommand command)
        => Cli.Call(Executable, command);

    public static Task<CommandOutput> CallAsync(Action<CliArgsCommandBuilder> configure)
    {
        var builder = new CliArgsCommandBuilder();
        configure(builder);
        return CallAsync(builder.Build());
    }

    public static Task<CommandOutput> CallAsync(CliArgsCommand command, CancellationToken cancellationToken = default)
        => Cli.CallAsync(Executable, command, cancellationToken);

    public static CommandOutput Script(string script, CliStartInfo startInfo)
        => Script(new BashScriptCommand(script, startInfo));

    public static CommandOutput Script(BashScriptCommand command)
        => Cli.CallScript(Executable, command);

    public static Task<CommandOutput> ScriptAsync(
        string script,
        CliStartInfo startInfo,
        CancellationToken cancellationToken = default)
        => ScriptAsync(new BashScriptCommand(script, startInfo), cancellationToken);

    public static Task<CommandOutput> ScriptAsync(BashScriptCommand command, CancellationToken cancellationToken = default)
        => Cli.CallScriptAsync(Executable, command, cancellationToken);
}
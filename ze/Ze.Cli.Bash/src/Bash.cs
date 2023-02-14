using System;
using System.Linq;

using Bearz.Cli.Execution;
using Bearz.Extensions.Cli;
using Bearz.Std;

namespace Ze.Cli.Bash;

public static partial class Bash
{
    public static Executable Executable { get; } = new Executable()
    {
        Name = "bash",
        Windows = new[]
        {
            "%ProgramFiles%\\Git\\bin\\bash.exe",
            "%ProgramFiles%\\Git\\usr\\bin\\bash.exe",
            "%WINDIR%\\System32\\bash.exe",
        },
    };

    public static CommandOutput Run(ICliCommand command)
        => Executable.Output(command);

    public static CommandOutput Run(CliArgsCommand command)
        => Executable.Output(command);

    public static CommandOutput Run(ICliCommandBuilder builder)
        => Executable.Output(builder);

    public static Task<CommandOutput> RunAsync(ICliCommandBuilder builder, CancellationToken cancellationToken = default)
        => Executable.OutputAsync(builder, cancellationToken);

    public static Task<CommandOutput> RunAsync(CliArgsCommand command, CancellationToken cancellationToken = default)
        => Executable.OutputAsync(command, cancellationToken);

    public static Task<CommandOutput> RunAsync(ICliCommand command, CancellationToken cancellationToken = default)
        => Executable.OutputAsync(command, cancellationToken);

    public static CommandOutput RunScript(string script, CliStartInfo? startInfo = null)
        => Executable.OutputScript(new BashScriptCommand(script, startInfo ?? new CliStartInfo()));

    public static CommandOutput RunScript(CliScriptCommand command)
        => Executable.OutputScript(command);

    public static Task<CommandOutput> RunScriptAsync(
        string script,
        CliStartInfo? startInfo = null,
        CancellationToken cancellationToken = default)
        => Executable.OutputScriptAsync(new BashScriptCommand(script, startInfo ?? new CliStartInfo()), cancellationToken);

    public static Task<CommandOutput> RunScriptAsync(
        CliScriptCommand command,
        CancellationToken cancellationToken = default)
        => Executable.OutputScriptAsync(command, cancellationToken);
}
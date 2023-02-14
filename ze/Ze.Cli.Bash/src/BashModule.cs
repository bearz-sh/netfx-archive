using Bearz.Cli.Execution;
using Bearz.Std;

using Ze.Cli.Bash;

namespace Ze.Cli.Bash;

public static class BashModule
{
    public static CommandOutput RunBash(ICliCommand command)
        => Bash.Executable.Output(command);

    public static CommandOutput RunBash(CliArgsCommand command)
        => Bash.Executable.Output(command);

    public static CommandOutput RunBash(ICliCommandBuilder builder)
        => Bash.Executable.Output(builder);

    public static Task<CommandOutput> RunBashAsync(ICliCommandBuilder builder, CancellationToken cancellationToken = default)
        => Bash.Executable.OutputAsync(builder, cancellationToken);

    public static Task<CommandOutput> RunBashAsync(CliArgsCommand command, CancellationToken cancellationToken = default)
        => Bash.Executable.OutputAsync(command, cancellationToken);

    public static Task<CommandOutput> RunBashAsync(ICliCommand command, CancellationToken cancellationToken = default)
        => Bash.Executable.OutputAsync(command, cancellationToken);

    public static CommandOutput RunBashScript(string script, CliStartInfo? startInfo = null)
        => Bash.Executable.OutputScript(new BashScriptCommand(script, startInfo ?? new CliStartInfo()));

    public static CommandOutput RunBashScript(CliScriptCommand command)
        => Bash.Executable.OutputScript(command);

    public static Task<CommandOutput> RunBashScriptAsync(
        string script,
        CliStartInfo? startInfo = null,
        CancellationToken cancellationToken = default)
        => Bash.Executable.OutputScriptAsync(new BashScriptCommand(script, startInfo ?? new CliStartInfo()), cancellationToken);

    public static Task<CommandOutput> RunBashScriptAsync(
        CliScriptCommand command,
        CancellationToken cancellationToken = default)
        => Bash.Executable.OutputScriptAsync(command, cancellationToken);
}
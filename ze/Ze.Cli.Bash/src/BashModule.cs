using Bearz.Cli.Execution;
using Bearz.Std;

using Ze.Cli.Bash;

namespace Ze.Cli.Bash;

public static class BashModule
{
    public static CommandOutput RunBash(ICliCommand command)
        => Bash.Executable.Call(command);

    public static CommandOutput RunBash(CliArgsCommand command)
        => Bash.Executable.Call(command);

    public static CommandOutput RunBash(ICliCommandBuilder builder)
        => Bash.Executable.Call(builder);

    public static Task<CommandOutput> RunBashAsync(ICliCommandBuilder builder, CancellationToken cancellationToken = default)
        => Bash.Executable.CallAsync(builder, cancellationToken);

    public static Task<CommandOutput> RunBashAsync(CliArgsCommand command, CancellationToken cancellationToken = default)
        => Bash.Executable.CallAsync(command, cancellationToken);

    public static Task<CommandOutput> RunBashAsync(ICliCommand command, CancellationToken cancellationToken = default)
        => Bash.Executable.CallAsync(command, cancellationToken);

    public static CommandOutput RunBashScript(string script, CliStartInfo? startInfo = null)
        => Bash.Executable.CallScript(new BashScriptCommand(script, startInfo ?? new CliStartInfo()));

    public static CommandOutput RunBashScript(CliScriptCommand command)
        => Bash.Executable.CallScript(command);

    public static Task<CommandOutput> RunBashScriptAsync(
        string script,
        CliStartInfo? startInfo = null,
        CancellationToken cancellationToken = default)
        => Bash.Executable.CallScriptAsync(new BashScriptCommand(script, startInfo ?? new CliStartInfo()), cancellationToken);

    public static Task<CommandOutput> RunBashScriptAsync(
        CliScriptCommand command,
        CancellationToken cancellationToken = default)
        => Bash.Executable.CallScriptAsync(command, cancellationToken);
}
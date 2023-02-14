using Bearz.Cli.Execution;
using Bearz.Extensions.Cli;
using Bearz.Std;

namespace Ze.Cli.Bash;

public static class BashCliExecutionContextExtensions
{
    public static CommandOutput RunBash(this ICliExecutionContext context, ICliCommand command)
        => Bash.Executable.Call(context, command);

    public static CommandOutput RunBash(this ICliExecutionContext context, CliArgsCommand command)
        => Bash.Executable.Call(context, command);

    public static CommandOutput RunBash(this ICliExecutionContext context, ICliCommandBuilder builder)
        => Bash.Executable.Call(context, builder.Build());

    public static Task<CommandOutput> RunBashAsync(
        this ICliExecutionContext context,
        ICliCommand command,
        CancellationToken cancellationToken = default)
        => Bash.Executable.CallAsync(context, command, cancellationToken);

    public static Task<CommandOutput> RunBashAsync(
        this ICliExecutionContext context,
        CliArgsCommand command,
        CancellationToken cancellationToken = default)
        => Bash.Executable.CallAsync(context, command, cancellationToken);

    public static Task<CommandOutput> RunBashAsync(
        this ICliExecutionContext context,
        ICliCommandBuilder builder,
        CancellationToken cancellationToken = default)
        => Bash.Executable.CallAsync(context, builder.Build(), cancellationToken);

    public static CommandOutput RunBashScript(this ICliExecutionContext context, CliScriptCommand command)
        => Bash.Executable.CallScript(context, command);

    public static CommandOutput RunBashScript(this ICliExecutionContext context, string script, CliStartInfo? startInfo = null)
        => Bash.Executable.CallScript(context, new BashScriptCommand(script, startInfo ?? new CliStartInfo()));

    public static Task<CommandOutput> RunBashScriptAsync(this ICliExecutionContext context, CliScriptCommand command)
        => Bash.Executable.CallScriptAsync(context, command);

    public static Task<CommandOutput> RunBashScriptAsync(this ICliExecutionContext context, string script, CliStartInfo? startInfo = null)
        => Bash.Executable.CallScriptAsync(context, new BashScriptCommand(script, startInfo ?? new CliStartInfo()));
}
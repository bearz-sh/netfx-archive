using Bearz.Cli.Execution;
using Bearz.Extensions.Cli;
using Bearz.Std;

namespace Ze.Cli.Bash;

public static class BashCliExecutionContextExtensions
{
    public static CommandOutput RunBash(this ICliExecutionContext context, ICliCommand command)
        => BashCli.Executable.Call(context, command);

    public static CommandOutput RunBash(this ICliExecutionContext context, CliArgsCommand command)
        => BashCli.Executable.Call(context, command);

    public static CommandOutput RunBash(this ICliExecutionContext context, ICliCommandBuilder builder)
        => BashCli.Executable.Call(context, builder.Build());

    public static Task<CommandOutput> RunBashAsync(
        this ICliExecutionContext context,
        ICliCommand command,
        CancellationToken cancellationToken = default)
        => BashCli.Executable.CallAsync(context, command, cancellationToken);

    public static Task<CommandOutput> RunBashAsync(
        this ICliExecutionContext context,
        CliArgsCommand command,
        CancellationToken cancellationToken = default)
        => BashCli.Executable.CallAsync(context, command, cancellationToken);

    public static Task<CommandOutput> RunBashAsync(
        this ICliExecutionContext context,
        ICliCommandBuilder builder,
        CancellationToken cancellationToken = default)
        => BashCli.Executable.CallAsync(context, builder.Build(), cancellationToken);

    public static CommandOutput RunBashScript(this ICliExecutionContext context, CliScriptCommand command)
        => BashCli.Executable.CallScript(context, command);

    public static CommandOutput RunBashScript(this ICliExecutionContext context, string script, CliStartInfo? startInfo = null)
        => BashCli.Executable.CallScript(context, new BashScriptCommand(script, startInfo ?? new CliStartInfo()));

    public static Task<CommandOutput> RunBashScriptAsync(this ICliExecutionContext context, CliScriptCommand command)
        => BashCli.Executable.CallScriptAsync(context, command);

    public static Task<CommandOutput> RunBashScriptAsync(this ICliExecutionContext context, string script, CliStartInfo? startInfo = null)
        => BashCli.Executable.CallScriptAsync(context, new BashScriptCommand(script, startInfo ?? new CliStartInfo()));
}
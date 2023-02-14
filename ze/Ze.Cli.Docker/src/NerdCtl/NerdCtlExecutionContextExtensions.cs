using Bearz.Cli.Execution;
using Bearz.Extensions.Cli;
using Bearz.Std;

namespace Ze.Cli.NerdCtl;

public static class NerdCtlExecutionContextExtensions
{
    public static CommandOutput RunNerdCtl(this ICliExecutionContext context, ICliCommand command)
        => NerdCtl.Executable.Call(context, command);

    public static CommandOutput RunNerdCtl(this ICliExecutionContext context, CliArgsCommand command)
        => NerdCtl.Executable.Call(context, command);

    public static CommandOutput RunNerdCtl(this ICliExecutionContext context, ICliCommandBuilder builder)
        => NerdCtl.Executable.Call(context, builder.Build());

    public static Task<CommandOutput> RunNerdCtlAsync(
        this ICliExecutionContext context,
        ICliCommand command,
        CancellationToken cancellationToken = default)
        => NerdCtl.Executable.CallAsync(context, command, cancellationToken);

    public static Task<CommandOutput> RunNerdCtlAsync(
        this ICliExecutionContext context,
        CliArgsCommand command,
        CancellationToken cancellationToken = default)
        => NerdCtl.Executable.CallAsync(context, command, cancellationToken);

    public static Task<CommandOutput> RunNerdCtlAsync(
        this ICliExecutionContext context,
        ICliCommandBuilder builder,
        CancellationToken cancellationToken = default)
        => NerdCtl.Executable.CallAsync(context, builder.Build(), cancellationToken);
}
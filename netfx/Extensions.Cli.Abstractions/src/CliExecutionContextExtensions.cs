using Bearz.Cli.Execution;
using Bearz.Std;

namespace Bearz.Extensions.Cli;

public static class CliExecutionContextExtensions
{
    public static CommandOutput Call(this ICliExecutionContext context, ExecutableInfo info, ICliCommand command)
    {
        return info.Call(context, command);
    }

    public static CommandOutput Call(this ICliExecutionContext context, ExecutableInfo info, ICliCommandBuilder builder)
    {
        return info.Call(context, builder.Build());
    }

    public static Task<CommandOutput> CallAsync(
        this ICliExecutionContext context,
        ExecutableInfo info,
        ICliCommand command,
        CancellationToken cancellationToken = default)
    {
        return info.CallAsync(context, command, cancellationToken);
    }

    public static Task<CommandOutput> CallAsync(
        this ICliExecutionContext context,
        ExecutableInfo info,
        ICliCommandBuilder builder,
        CancellationToken cancellationToken = default)
    {
        return info.CallAsync(context, builder.Build(), cancellationToken);
    }
}
using Bearz.Cli.Execution;
using Bearz.Extensions.Cli;
using Bearz.Std;

namespace Ze.Cli.Apt;

public static class AptCliExecutionContextExtensions
{
    public static CommandOutput RunApt(this ICliExecutionContext context, ICliCommand command)
        => Apt.Executable.Call(context, command);

    public static CommandOutput RunApt(this ICliExecutionContext context, CliArgsCommand command)
        => Apt.Executable.Call(context, command);

    public static CommandOutput RunApt(this ICliExecutionContext context, ICliCommandBuilder builder)
        => Apt.Executable.Call(context, builder.Build());

    public static Task<CommandOutput> RunAptAsync(
        this ICliExecutionContext context,
        ICliCommand command,
        CancellationToken cancellationToken = default)
        => Apt.Executable.CallAsync(context, command, cancellationToken);

    public static Task<CommandOutput> RunAptAsync(
        this ICliExecutionContext context,
        CliArgsCommand command,
        CancellationToken cancellationToken = default)
        => Apt.Executable.CallAsync(context, command, cancellationToken);

    public static Task<CommandOutput> RunAptAsync(
        this ICliExecutionContext context,
        ICliCommandBuilder builder,
        CancellationToken cancellationToken = default)
        => Apt.Executable.CallAsync(context, builder.Build(), cancellationToken);
}
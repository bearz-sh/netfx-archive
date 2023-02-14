using Bearz.Cli.Execution;
using Bearz.Extensions.Cli;
using Bearz.Std;

namespace Ze.Cli.Chocolatey;

public static class ChocolateyCliExecutionContextExtensions
{
    public static CommandOutput RunChocolatey(this ICliExecutionContext context, ICliCommand command)
        => Chocolatey.Executable.Call(context, command);

    public static CommandOutput RunChocolatey(this ICliExecutionContext context, CliArgsCommand command)
        => Chocolatey.Executable.Call(context, command);

    public static CommandOutput RunChocolatey(this ICliExecutionContext context, ICliCommandBuilder builder)
        => Chocolatey.Executable.Call(context, builder.Build());

    public static Task<CommandOutput> RunChocolateyAsync(
        this ICliExecutionContext context,
        ICliCommand command,
        CancellationToken cancellationToken = default)
        => Chocolatey.Executable.CallAsync(context, command, cancellationToken);

    public static Task<CommandOutput> RunChocolateyAsync(
        this ICliExecutionContext context,
        CliArgsCommand command,
        CancellationToken cancellationToken = default)
        => Chocolatey.Executable.CallAsync(context, command, cancellationToken);

    public static Task<CommandOutput> RunChocolateyAsync(
        this ICliExecutionContext context,
        ICliCommandBuilder builder,
        CancellationToken cancellationToken = default)
        => Chocolatey.Executable.CallAsync(context, builder.Build(), cancellationToken);
}
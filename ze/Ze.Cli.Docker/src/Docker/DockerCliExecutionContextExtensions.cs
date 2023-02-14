using Bearz.Cli.Execution;
using Bearz.Extensions.Cli;
using Bearz.Std;

namespace Ze.Cli.Docker;

public static class DockerCliExecutionContextExtensions
{
    public static CommandOutput RunDocker(this ICliExecutionContext context, ICliCommand command)
        => Docker.Executable.Call(context, command);

    public static CommandOutput RunDocker(this ICliExecutionContext context, CliArgsCommand command)
        => Docker.Executable.Call(context, command);

    public static CommandOutput RunDocker(this ICliExecutionContext context, ICliCommandBuilder builder)
        => Docker.Executable.Call(context, builder.Build());

    public static Task<CommandOutput> RunDockerAsync(
        this ICliExecutionContext context,
        ICliCommand command,
        CancellationToken cancellationToken = default)
        => Docker.Executable.CallAsync(context, command, cancellationToken);

    public static Task<CommandOutput> RunDockerAsync(
        this ICliExecutionContext context,
        CliArgsCommand command,
        CancellationToken cancellationToken = default)
        => Docker.Executable.CallAsync(context, command, cancellationToken);

    public static Task<CommandOutput> RunDockerAsync(
        this ICliExecutionContext context,
        ICliCommandBuilder builder,
        CancellationToken cancellationToken = default)
        => Docker.Executable.CallAsync(context, builder.Build(), cancellationToken);
}
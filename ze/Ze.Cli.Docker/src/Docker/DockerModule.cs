using Bearz.Cli.Execution;
using Bearz.Std;

namespace Ze.Cli.Docker;

public static class DockerModule
{
    public static CommandOutput RunDocker(ICliCommand command)
        => Docker.Executable.Call(command);

    public static CommandOutput RunDocker(CliArgsCommand command)
        => Docker.Executable.Call(command);

    public static CommandOutput RunDocker(ICliCommandBuilder builder)
        => Docker.Executable.Call(builder);

    public static Task<CommandOutput> RunDockerAsync(ICliCommandBuilder builder, CancellationToken cancellationToken = default)
        => Docker.Executable.CallAsync(builder, cancellationToken);

    public static Task<CommandOutput> RunDockerAsync(CliArgsCommand command, CancellationToken cancellationToken = default)
        => Docker.Executable.CallAsync(command, cancellationToken);

    public static Task<CommandOutput> RunDockerAsync(ICliCommand command, CancellationToken cancellationToken = default)
        => Docker.Executable.CallAsync(command, cancellationToken);
}
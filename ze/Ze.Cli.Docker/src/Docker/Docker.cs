using Bearz.Cli.Execution;
using Bearz.Extra.Collections;
using Bearz.Std;

using Ze.Cli;

namespace Ze.Cli.Docker;

public static partial class Docker
{
    public static ExecutableInfo Executable { get; set; } = new ExecutableInfo
    {
        Name = "docker",
        Windows = new[] { "%Program Files%\\Docker\\Docker\\resources\\bin\\docker.exe", },
        Linux = new[] { "usr/bin/docker" },
    };

    public static CommandOutput Run(ICliCommand command)
        => Executable.Call(command);

    public static CommandOutput Run(CliArgsCommand command)
        => Executable.Call(command);

    public static CommandOutput Run(ICliCommandBuilder builder)
        => Executable.Call(builder);

    public static Task<CommandOutput> RunAsync(ICliCommandBuilder builder, CancellationToken cancellationToken = default)
        => Executable.CallAsync(builder, cancellationToken);

    public static Task<CommandOutput> RunAsync(CliArgsCommand command, CancellationToken cancellationToken = default)
        => Executable.CallAsync(command, cancellationToken);

    public static Task<CommandOutput> RunAsync(ICliCommand command, CancellationToken cancellationToken = default)
        => Executable.CallAsync(command, cancellationToken);
}
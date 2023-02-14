using System;
using System.Linq;

using Bearz.Cli.Execution;
using Bearz.Std;

namespace Ze.Cli.Apt;

public static class Apt
{
    public static ExecutableInfo Executable { get; } = new ExecutableInfo
    {
        Name = "apt",
        Linux = new[]
        {
            "/usr/bin/apt",
        },
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
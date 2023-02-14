using Bearz.Cli.Execution;
using Bearz.Std;

// ReSharper disable once CheckNamespace
namespace Ze.Cli.NerdCtl;

public static class NerdCtl
{
    public static ExecutableInfo Executable { get; set; } = new ExecutableInfo
    {
        Name = "nerdctl",
        Linux = new[] { "usr/bin/nertctl", },
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
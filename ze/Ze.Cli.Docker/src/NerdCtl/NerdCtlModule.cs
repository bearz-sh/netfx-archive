using Bearz.Cli.Execution;
using Bearz.Std;

// ReSharper disable once CheckNamespace
namespace Ze.Cli.NerdCtl;

public static class NerdCtlModule
{
    public static CommandOutput RunNerdCtl(ICliCommand command)
        => NerdCtl.Executable.Call(command);

    public static CommandOutput RunNerdCtl(CliArgsCommand command)
        => NerdCtl.Executable.Call(command);

    public static CommandOutput RunNerdCtl(ICliCommandBuilder builder)
        => NerdCtl.Executable.Call(builder);

    public static Task<CommandOutput> RunNerdCtlAsync(ICliCommandBuilder builder, CancellationToken cancellationToken = default)
        => NerdCtl.Executable.CallAsync(builder, cancellationToken);

    public static Task<CommandOutput> RunNerdCtlAsync(CliArgsCommand command, CancellationToken cancellationToken = default)
        => NerdCtl.Executable.CallAsync(command, cancellationToken);

    public static Task<CommandOutput> RunNerdCtlAsync(ICliCommand command, CancellationToken cancellationToken = default)
        => NerdCtl.Executable.CallAsync(command, cancellationToken);
}
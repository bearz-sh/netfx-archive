using Bearz.Cli.Execution;
using Bearz.Std;

namespace Ze.Cli.Apt;

public static class AptModule
{
    public static CommandOutput RunApt(ICliCommand command)
        => Apt.Executable.Call(command);

    public static CommandOutput RunApt(CliArgsCommand command)
        => Apt.Executable.Call(command);

    public static CommandOutput RunApt(ICliCommandBuilder builder)
        => Apt.Executable.Call(builder);

    public static Task<CommandOutput> RunAptAsync(ICliCommandBuilder builder, CancellationToken cancellationToken = default)
        => Apt.Executable.CallAsync(builder, cancellationToken);

    public static Task<CommandOutput> RunAptAsync(CliArgsCommand command, CancellationToken cancellationToken = default)
        => Apt.Executable.CallAsync(command, cancellationToken);

    public static Task<CommandOutput> RunAptAsync(ICliCommand command, CancellationToken cancellationToken = default)
        => Apt.Executable.CallAsync(command, cancellationToken);
}
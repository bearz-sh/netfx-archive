using Bearz.Cli.Execution;
using Bearz.Std;

namespace Ze.Cli.Chocolatey;

public static class ChocolateyModule
{
    public static CommandOutput RunChocolatey(ICliCommand command)
        => Chocolatey.Executable.Call(command);

    public static CommandOutput RunChocolatey(CliArgsCommand command)
        => Chocolatey.Executable.Call(command);

    public static CommandOutput RunChocolatey(ICliCommandBuilder builder)
        => Chocolatey.Executable.Call(builder);

    public static Task<CommandOutput> RunChocolateyAsync(ICliCommandBuilder builder, CancellationToken cancellationToken = default)
        => Chocolatey.Executable.CallAsync(builder, cancellationToken);

    public static Task<CommandOutput> RunChocolateyAsync(CliArgsCommand command, CancellationToken cancellationToken = default)
        => Chocolatey.Executable.CallAsync(command, cancellationToken);

    public static Task<CommandOutput> RunChocolateyAsync(ICliCommand command, CancellationToken cancellationToken = default)
        => Chocolatey.Executable.CallAsync(command, cancellationToken);
}
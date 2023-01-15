using System;
using System.Linq;

using Bearz.Std;

namespace Ze.Cli.Apt;

public static class Apt
{
    public static ExecutableInfo Executable { get; } = new ExecutableInfo
    {
        Name = "apt",
    };

    public static CommandOutput Call(Action<CliArgsCommandBuilder> configure)
    {
        var builder = new CliArgsCommandBuilder();
        configure(builder);
        return Call(builder.Build());
    }

    public static CommandOutput Call(CliArgsCommand command)
        => Cli.Call(Executable, command);

    public static Task<CommandOutput> CallAsync(Action<CliArgsCommandBuilder> configure, CancellationToken cancellationToken = default)
    {
        var builder = new CliArgsCommandBuilder();
        configure(builder);
        return CallAsync(builder.Build(), cancellationToken);
    }

    public static Task<CommandOutput> CallAsync(CliArgsCommand command, CancellationToken cancellationToken = default)
        => Cli.CallAsync(Executable, command, cancellationToken);
}
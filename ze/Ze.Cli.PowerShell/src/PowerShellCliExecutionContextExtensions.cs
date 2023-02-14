using Bearz.Cli.Execution;
using Bearz.Extensions.Cli;
using Bearz.Std;

namespace Ze.Cli.PowerShell;

public static class PowerShellCliExecutionContextExtensions
{
    public static CommandOutput RunPowerShell(this ICliExecutionContext context, ICliCommand command)
        => PowerShell.Executable.Call(context, command);

    public static CommandOutput RunPowerShell(this ICliExecutionContext context, CliArgsCommand command)
        => PowerShell.Executable.Call(context, command);

    public static CommandOutput RunPowerShell(this ICliExecutionContext context, ICliCommandBuilder builder)
        => PowerShell.Executable.Call(context, builder.Build());

    public static Task<CommandOutput> RunPowerShellAsync(
        this ICliExecutionContext context,
        ICliCommand command,
        CancellationToken cancellationToken = default)
        => PowerShell.Executable.CallAsync(context, command, cancellationToken);

    public static Task<CommandOutput> RunPowerShellAsync(
        this ICliExecutionContext context,
        CliArgsCommand command,
        CancellationToken cancellationToken = default)
        => PowerShell.Executable.CallAsync(context, command, cancellationToken);

    public static Task<CommandOutput> RunPowerShellAsync(
        this ICliExecutionContext context,
        ICliCommandBuilder builder,
        CancellationToken cancellationToken = default)
        => PowerShell.Executable.CallAsync(context, builder.Build(), cancellationToken);

    public static CommandOutput RunPowerShellScript(this ICliExecutionContext context, CliScriptCommand command)
        => PowerShell.Executable.CallScript(context, command);

    public static CommandOutput RunPowerShellScript(this ICliExecutionContext context, string script, CliStartInfo? startInfo = null)
        => PowerShell.Executable.CallScript(context, new PwshScriptCommand(script, startInfo ?? new CliStartInfo()));

    public static Task<CommandOutput> RunPowerShellScriptAsync(this ICliExecutionContext context, CliScriptCommand command)
        => PowerShell.Executable.CallScriptAsync(context, command);

    public static Task<CommandOutput> RunPowerShellScriptAsync(this ICliExecutionContext context, string script, CliStartInfo? startInfo = null)
        => PowerShell.Executable.CallScriptAsync(context, new PwshScriptCommand(script, startInfo ?? new CliStartInfo()));
}
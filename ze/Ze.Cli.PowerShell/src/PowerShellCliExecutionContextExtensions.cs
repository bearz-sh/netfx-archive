using Bearz.Cli.Execution;
using Bearz.Extensions.Cli;
using Bearz.Std;

namespace Ze.Cli.PowerShell;

public static class PowerShellCliExecutionContextExtensions
{
    public static CommandOutput RunPowerShell(this ICliExecutionContext context, ICliCommand command)
        => PowerShellCli.Executable.Call(context, command);

    public static CommandOutput RunPowerShell(this ICliExecutionContext context, CliArgsCommand command)
        => PowerShellCli.Executable.Call(context, command);

    public static CommandOutput RunPowerShell(this ICliExecutionContext context, ICliCommandBuilder builder)
        => PowerShellCli.Executable.Call(context, builder.Build());

    public static Task<CommandOutput> RunPowerShellAsync(
        this ICliExecutionContext context,
        ICliCommand command,
        CancellationToken cancellationToken = default)
        => PowerShellCli.Executable.CallAsync(context, command, cancellationToken);

    public static Task<CommandOutput> RunPowerShellAsync(
        this ICliExecutionContext context,
        CliArgsCommand command,
        CancellationToken cancellationToken = default)
        => PowerShellCli.Executable.CallAsync(context, command, cancellationToken);

    public static Task<CommandOutput> RunPowerShellAsync(
        this ICliExecutionContext context,
        ICliCommandBuilder builder,
        CancellationToken cancellationToken = default)
        => PowerShellCli.Executable.CallAsync(context, builder.Build(), cancellationToken);

    public static CommandOutput RunPowerShellScript(this ICliExecutionContext context, CliScriptCommand command)
        => PowerShellCli.Executable.CallScript(context, command);

    public static CommandOutput RunPowerShellScript(this ICliExecutionContext context, string script, CliStartInfo? startInfo = null)
        => PowerShellCli.Executable.CallScript(context, new PwshScriptCommand(script, startInfo ?? new CliStartInfo()));

    public static Task<CommandOutput> RunPowerShellScriptAsync(this ICliExecutionContext context, CliScriptCommand command)
        => PowerShellCli.Executable.CallScriptAsync(context, command);

    public static Task<CommandOutput> RunPowerShellScriptAsync(this ICliExecutionContext context, string script, CliStartInfo? startInfo = null)
        => PowerShellCli.Executable.CallScriptAsync(context, new PwshScriptCommand(script, startInfo ?? new CliStartInfo()));
}
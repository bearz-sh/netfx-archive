using Bearz.Cli.Execution;
using Bearz.Std;

namespace Ze.Cli.PowerShell;

public static class PowerShellModule
{
    public static CommandOutput RunPowerShell(ICliCommand command)
        => PowerShell.Executable.Call(command);

    public static CommandOutput RunPowerShell(CliArgsCommand command)
        => PowerShell.Executable.Call(command);

    public static CommandOutput RunPowerShell(ICliCommandBuilder builder)
        => PowerShell.Executable.Call(builder);

    public static Task<CommandOutput> RunPowerShellAsync(ICliCommandBuilder builder, CancellationToken cancellationToken = default)
        => PowerShell.Executable.CallAsync(builder, cancellationToken);

    public static Task<CommandOutput> RunPowerShellAsync(CliArgsCommand command, CancellationToken cancellationToken = default)
        => PowerShell.Executable.CallAsync(command, cancellationToken);

    public static Task<CommandOutput> RunPowerShellAsync(ICliCommand command, CancellationToken cancellationToken = default)
        => PowerShell.Executable.CallAsync(command, cancellationToken);

    public static CommandOutput RunPowerShellScript(string script, CliStartInfo? startInfo = null)
        => PowerShell.Executable.CallScript(new PwshScriptCommand(script, startInfo ?? new CliStartInfo()));

    public static CommandOutput RunPowerShellScript(CliScriptCommand command)
        => PowerShell.Executable.CallScript(command);

    public static Task<CommandOutput> RunPowerShellScriptAsync(
        string script,
        CliStartInfo? startInfo = null,
        CancellationToken cancellationToken = default)
        => PowerShell.Executable.CallScriptAsync(new PwshScriptCommand(script, startInfo ?? new CliStartInfo()), cancellationToken);

    public static Task<CommandOutput> RunPowerShellScriptAsync(
        CliScriptCommand command,
        CancellationToken cancellationToken = default)
        => PowerShell.Executable.CallScriptAsync(command, cancellationToken);
}
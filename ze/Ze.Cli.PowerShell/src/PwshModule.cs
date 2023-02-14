using Bearz.Cli.Execution;
using Bearz.Std;

namespace Ze.Cli.PowerShell;

public static class PwshModule
{
    public static CommandOutput RunPwsh(ICliCommand command)
        => Pwsh.Executable.Call(command);

    public static CommandOutput RunPwsh(CliArgsCommand command)
        => Pwsh.Executable.Call(command);

    public static CommandOutput RunPwsh(ICliCommandBuilder builder)
        => Pwsh.Executable.Call(builder);

    public static Task<CommandOutput> RunPwshAsync(ICliCommandBuilder builder, CancellationToken cancellationToken = default)
        => Pwsh.Executable.CallAsync(builder, cancellationToken);

    public static Task<CommandOutput> RunPwshAsync(CliArgsCommand command, CancellationToken cancellationToken = default)
        => Pwsh.Executable.CallAsync(command, cancellationToken);

    public static Task<CommandOutput> RunPwshAsync(ICliCommand command, CancellationToken cancellationToken = default)
        => Pwsh.Executable.CallAsync(command, cancellationToken);

    public static CommandOutput RunPwshScript(string script, CliStartInfo? startInfo = null)
        => Pwsh.Executable.CallScript(new PwshScriptCommand(script, startInfo ?? new CliStartInfo()));

    public static CommandOutput RunPwshScript(CliScriptCommand command)
        => Pwsh.Executable.CallScript(command);

    public static Task<CommandOutput> RunPwshScriptAsync(
        string script,
        CliStartInfo? startInfo = null,
        CancellationToken cancellationToken = default)
        => Pwsh.Executable.CallScriptAsync(new PwshScriptCommand(script, startInfo ?? new CliStartInfo()), cancellationToken);

    public static Task<CommandOutput> RunPwshScriptAsync(
        CliScriptCommand command,
        CancellationToken cancellationToken = default)
        => Pwsh.Executable.CallScriptAsync(command, cancellationToken);
}
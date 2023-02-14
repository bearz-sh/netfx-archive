using Bearz.Cli.Execution;
using Bearz.Std;

namespace Ze.Cli.PowerShell;

public static class PowerShell
{
    public static ExecutableInfo Executable { get; } = new ExecutableInfo
    {
        Name = "powershell",
        Windows = new[]
        {
            "%SystemRoot%\\System32\\WindowsPowerShell\\v1.0\\powershell.exe",
            "%ProgramFiles%\\PowerShell\\7\\pwsh.exe",
            "%ProgramFiles(x86)%\\PowerShell\\7\\pwsh.exe",
            "%ProgramFiles%\\PowerShell\\6\\pwsh.exe",
            "%ProgramFiles(x86)%\\PowerShell\\6\\pwsh.exe",
        },
        Linux = new[]
        {
            "/opt/microsoft/powershell/7/pwsh",
            "/opt/microsoft/powershell/6/pwsh",
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

    public static CommandOutput RunScript(string script, CliStartInfo startInfo)
        => Executable.CallScript(new PwshScriptCommand(script, startInfo));

    public static CommandOutput RunScript(CliScriptCommand command)
        => Executable.CallScript(command);

    public static Task<CommandOutput> RunScriptAsync(
        string script,
        CliStartInfo startInfo,
        CancellationToken cancellationToken = default)
        => Executable.CallScriptAsync(new PwshScriptCommand(script, startInfo), cancellationToken);

    public static Task<CommandOutput> RunScriptAsync(
        CliScriptCommand command,
        CancellationToken cancellationToken = default)
        => Executable.CallScriptAsync(command, cancellationToken);
}
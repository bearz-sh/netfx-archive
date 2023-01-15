using Bearz.Std;

namespace Ze.Cli.PowerShell;

public static class Pwsh
{
    public static ExecutableInfo Executable { get; } = new ExecutableInfo
    {
        Name = "pwsh",
        Windows = new[]
        {
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

    public static CommandOutput Run(CliArgsCommand command)
        => Cli.Call(Executable, command);

    public static Task<CommandOutput> RunAsync(CliArgsCommand command, CancellationToken cancellationToken = default)
        => Cli.CallAsync(Executable, command, cancellationToken);

    public static CommandOutput Script(string script, CliStartInfo startInfo)
        => Script(new PwshScriptCommand(script, startInfo));

    public static CommandOutput Script(PwshScriptCommand command)
        => Cli.CallScript(Executable, command);

    public static Task<CommandOutput> ScriptAsync(
        string script,
        CliStartInfo startInfo,
        CancellationToken cancellationToken = default)
        => ScriptAsync(new PwshScriptCommand(script, startInfo), cancellationToken);

    public static Task<CommandOutput> ScriptAsync(PwshScriptCommand command, CancellationToken cancellationToken = default)
        => Cli.CallScriptAsync(Executable, command, cancellationToken);
}
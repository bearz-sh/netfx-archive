using Bearz.Cli.Execution;

namespace Ze.Cli.PowerShell;

public class PowerShellExe : ShellExecutable
{
    public PowerShellExe(ICliExecutionContext? context = null)
        : base(context)
    {
        this.Name = "powershell";
        this.Windows = new[]
        {
            "%SystemRoot%\\System32\\WindowsPowerShell\\v1.0\\powershell.exe",
            "%ProgramFiles%\\PowerShell\\7\\pwsh.exe", "%ProgramFiles(x86)%\\PowerShell\\7\\pwsh.exe",
            "%ProgramFiles%\\PowerShell\\6\\pwsh.exe", "%ProgramFiles(x86)%\\PowerShell\\6\\pwsh.exe",
        };
        this.Linux = new[]
        {
            "/opt/microsoft/powershell/7/pwsh",
            "/opt/microsoft/powershell/6/pwsh",
        };
    }

    public static PowerShellExe Default { get; } = new();

    protected override CliScriptCommand CreateScriptCommand(string script, CliStartInfo? cliStartInfo = null)
        => new PowerShellScriptCommand(script, cliStartInfo);
}
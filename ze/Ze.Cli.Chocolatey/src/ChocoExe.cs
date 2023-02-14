using Bearz.Cli.Execution;

namespace Ze.Cli.Chocolatey;

public class ChocoExe : Executable
{
    public ChocoExe(ICliExecutionContext? context = null)
        : base(context)
    {
        this.Name = "choco";
        this.Windows = new[]
        {
            "%ChocolateyInstall%\\bin\\choco.exe",
            "%ALLUSERSPROFILE%\\chocolatey\\bin\\choco.exe",
        };
    }
}
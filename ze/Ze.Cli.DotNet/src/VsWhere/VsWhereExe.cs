using Bearz.Cli.Execution;

namespace Ze.Cli.VsWhere;

public class VsWhereExe : Executable
{
    public VsWhereExe(ICliExecutionContext? context = null)
        : base(context)
    {
        this.Name = "vswhere";
        this.Windows = new[]
        {
            "%ChocolateyInstall%\\bin\\vswhere.exe",
            "%ProgramFiles(x86)%\\Microsoft Visual Studio\\Installer\\vswhere.exe",
        };
    }

    public static VsWhereExe Default { get; } = new VsWhereExe();
}
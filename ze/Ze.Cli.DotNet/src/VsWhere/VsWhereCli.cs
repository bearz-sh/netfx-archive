using Bearz.Cli.Execution;

namespace Ze.Cli.VsWhere;

public class VsWhereCli : Executable
{
    public VsWhereCli(ICliExecutionContext? context = null)
        : base(context)
    {
        this.Name = "vswhere";
        this.Windows = new[]
        {
            "%ChocolateyInstall%\\bin\\vswhere.exe",
            "%ProgramFiles(x86)%\\Microsoft Visual Studio\\Installer\\vswhere.exe",
        };
    }

    public static VsWhereCli Default { get; } = new VsWhereCli();
}
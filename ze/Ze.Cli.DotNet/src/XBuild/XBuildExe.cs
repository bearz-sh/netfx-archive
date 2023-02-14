using Bearz.Cli.Execution;

namespace Ze.Cli.XBuild;

public class XBuildExe : Executable
{
    public XBuildExe(ICliExecutionContext? context = null)
        : base(context)
    {
        this.Name = "xbuild";
        this.Linux = new[] { "/usr/bin/xbuild" };
    }
}
using Bearz.Cli.Execution;

namespace Ze.Cli.Apt;

public class AptExe : Executable
{
    public AptExe(ICliExecutionContext? context = null)
        : base(context)
    {
        this.Name = "apt";
        this.Linux = new[] { "/usr/bin/apt", };
    }

    public static AptExe Default { get; } = new AptExe();
}
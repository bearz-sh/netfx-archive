using Bearz.Cli.Execution;

namespace Ze.Cli.Docker;

public class NerdCtlExe : Executable
{
    public NerdCtlExe(ICliExecutionContext? context = null)
        : base(context)
    {
        this.Name = "nerdctl";
        this.Linux = new[] { "usr/bin/nertctl", };
    }

    public static NerdCtlExe Default { get; } = new NerdCtlExe();
}
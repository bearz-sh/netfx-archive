using Bearz.Cli.Execution;

namespace Ze.Cli.NuGet;

public class NuGetExe : Executable
{
    public NuGetExe(ICliExecutionContext? context = null)
        : base(context)
    {
        this.Name = "nuget";
        this.Linux = new[] { "/usr/bin/nuget" };
    }

    public static NuGetExe Default { get; } = new();
}
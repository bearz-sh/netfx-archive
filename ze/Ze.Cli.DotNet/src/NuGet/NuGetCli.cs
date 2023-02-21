using Bearz.Cli.Execution;

namespace Ze.Cli.NuGet;

public class NuGetCli : Executable
{
    public NuGetCli(ICliExecutionContext? context = null)
        : base(context)
    {
        this.Name = "nuget";
        this.Linux = new[] { "/usr/bin/nuget" };
    }

    public static NuGetCli Default { get; } = new();
}
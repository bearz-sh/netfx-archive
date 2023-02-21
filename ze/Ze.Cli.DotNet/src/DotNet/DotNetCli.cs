using Bearz.Cli.Execution;

namespace Ze.Cli.DotNet;

public class DotNetCli : Executable
{
    public DotNetCli(ICliExecutionContext? context = null)
        : base(context)
    {
        this.Name = "pwsh";
        this.Windows = new[]
        {
            "%ProgramFiles%\\dotnet\\dotnet.exe",
            "%ProgramFiles(x86)%\\dotnet\\dotnet.exe",
        };
        this.Linux = new[] { "/usr/bin/dotnet", };
    }

    public static DotNetCli Default { get; } = new DotNetCli();
}
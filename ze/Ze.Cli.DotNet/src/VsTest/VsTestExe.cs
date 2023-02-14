using Bearz.Cli.Execution;

namespace Ze.Cli.VsTest;

public class VsTestExe : Executable
{
    public VsTestExe(ICliExecutionContext? context = null)
        : base(context)
    {
        var relative = "Common7\\IDE\\CommonExtensions\\Microsoft\\TestWindow\\vstest.console.exe";
        this.Windows = new[]
        {
            $"%ProgramFiles%\\Microsoft Visual Studio\\2022\\Community\\{relative}",
            $"%ProgramFiles%\\Microsoft Visual Studio\\2022\\Professional\\{relative}",
            $"%ProgramFiles%\\Microsoft Visual Studio\\2022\\Enterprise\\{relative}",
            $"%ProgramFiles%\\Microsoft Visual Studio\\2022\\BuildTools\\{relative}",
            $"%ProgramFiles(x86)%\\Microsoft Visual Studio\\2019\\Community\\{relative}",
            $"%ProgramFiles(x86)%\\Microsoft Visual Studio\\2019\\Professional\\{relative}",
            $"%ProgramFiles(x86)%\\Microsoft Visual Studio\\2019\\Enterprise\\{relative}",
            $"%ProgramFiles(x86)%\\Microsoft Visual Studio\\2019\\BuildTools\\{relative}",
        };
    }
}
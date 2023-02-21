using Bearz.Cli.Execution;

namespace Ze.Cli.MsBuild;

public class MsBuildCli : Executable
{
    public MsBuildCli()
    {
        this.Name = "MsBuild";
        this.Windows = new[]
        {
            "%ProgramFiles%\\Microsoft Visual Studio\\2022\\Community\\MSBuild\\Current\\Bin\\MsBuild.exe",
            "%ProgramFiles%\\Microsoft Visual Studio\\2022\\Professional\\MSBuild\\Current\\Bin\\MsBuild.exe",
            "%ProgramFiles%\\Microsoft Visual Studio\\2022\\Enterprise\\MSBuild\\Current\\Bin\\MsBuild.exe",
            "%ProgramFiles%\\Microsoft Visual Studio\\2022\\BuildTools\\MSBuild\\Current\\Bin\\MsBuild.exe",
            "%ProgramFiles(x86)%\\Microsoft Visual Studio\\2019\\Community\\MSBuild\\Current\\Bin\\MsBuild.exe",
            "%ProgramFiles(x86)%\\Microsoft Visual Studio\\2019\\Professional\\MSBuild\\Current\\Bin\\MsBuild.exe",
            "%ProgramFiles(x86)%\\Microsoft Visual Studio\\2019\\Enterprise\\MSBuild\\Current\\Bin\\MsBuild.exe",
            "%ProgramFiles(x86)%\\Microsoft Visual Studio\\2019\\BuildTools\\MSBuild\\Current\\Bin\\MsBuild.exe",
        };
        this.Linux = new[] { "/usr/bin/MsBuild", };
    }
}
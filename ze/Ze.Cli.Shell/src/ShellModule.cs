using Ze.Cli.Bash;
using Ze.Cli.PowerShell;
using Ze.Cli.Pwsh;
using Ze.Cli.Sh;

// ReSharper disable once CheckNamespace
namespace Ze.Cli;

public static class ShellModule
{
    public static BashExe Bash { get; } = BashExe.Default;

    public static PowerShellExe PowerShell { get; } = PowerShellExe.Default;

    public static PwshExe Pwsh { get; } = PwshExe.Default;

    public static ShExe Sh { get; } = ShExe.Default;
}
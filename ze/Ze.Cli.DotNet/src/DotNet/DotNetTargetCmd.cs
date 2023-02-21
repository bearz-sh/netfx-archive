using Bearz.Cli.Execution;

namespace Ze.Cli.DotNet;

public abstract class DotNetTargetCmd : DotNetCmd
{
    protected DotNetTargetCmd(string commandName, CliStartInfo? startInfo = null)
        : base(commandName, startInfo)
    {
    }

    public string Target { get; set; } = string.Empty;

    public string? Framework { get; set; }

    public string? Configuration { get; set; }

    public string? Runtime { get; set; }

    public bool NoLogo { get; set; }

    public string? Verbosity { get; set; }

    public string? Output { get; set; }
}
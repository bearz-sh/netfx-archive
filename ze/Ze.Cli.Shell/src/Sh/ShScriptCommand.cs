using Bearz.Cli.Execution;

namespace Ze.Cli.Sh;

public class ShScriptCommand : CliScriptCommand
{
    public ShScriptCommand(string script, CliStartInfo? cliStartInfo = null)
        : base(script, cliStartInfo)
    {
        this.Args.Add("-e");
    }

    protected override string Extension => ".sh";
}
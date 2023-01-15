using FluentBuilder;

namespace Ze.Cli.Bash;

[AutoGenerateBuilder]
public class ShScriptCommand : CliScriptCommand
{
    public ShScriptCommand()
    {
        this.Args.Add("-e");
    }

    public ShScriptCommand(string script)
        : this()
    {
        this.Script = script;
    }

    public ShScriptCommand(string script, CliStartInfo startInfo)
    {
        this.Script = script;
        this.CliStartInfo = startInfo;
        this.Args.Add("-e");
    }

    protected override string Extension => ".sh";
}
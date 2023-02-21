using Bearz.Std;

namespace Bearz.Cli.Execution;

public abstract class CliCommandBase : ICliCommand
{
    protected CliCommandBase(string commandName, CliStartInfo? startInfo = null)
    {
        this.CommandName = commandName;
        this.CliStartInfo = startInfo ?? new CliStartInfo();
    }

    public string CommandName { get; }

    public CliStartInfo CliStartInfo { get; }

    protected CommandArgs Args => this.CliStartInfo.Args;

    public abstract CommandStartInfo Build();
}
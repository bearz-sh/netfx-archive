using Bearz.Cli.Execution;
using Bearz.Std;

namespace Ze.Cli.DotNet;

public abstract class DotNetCmd : CliCommandBase
{
    protected DotNetCmd(string commandName, CliStartInfo? startInfo = null)
        : base(commandName, startInfo)
    {
    }

    /// <summary>
    /// Show command line help.
    /// </summary>
    public bool Help { get; set; }
}
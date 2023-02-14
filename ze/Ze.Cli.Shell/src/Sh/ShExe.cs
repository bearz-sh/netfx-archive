using Bearz.Cli.Execution;

namespace Ze.Cli.Sh;

public class ShExe : ShellExecutable
{
    public ShExe(ICliExecutionContext? context = null)
        : base(context)
    {
    }

    public static ShExe Default { get; } = new();

    protected override CliScriptCommand CreateScriptCommand(string script, CliStartInfo? cliStartInfo = null)
    {
        throw new NotImplementedException();
    }
}
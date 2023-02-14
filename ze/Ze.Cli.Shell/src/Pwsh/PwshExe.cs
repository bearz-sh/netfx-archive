using Bearz.Cli.Execution;

namespace Ze.Cli.Pwsh;

public class PwshExe : ShellExecutable
{
    public static PwshExe Default { get; } = new();

    protected override CliScriptCommand CreateScriptCommand(string script, CliStartInfo? cliStartInfo = null)
        => new PwshScriptCommand(script, cliStartInfo);
}
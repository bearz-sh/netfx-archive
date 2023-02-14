using Bearz.Cli.Execution;

namespace Ze.Cli.Bash;

public class BashExe : ShellExecutable
{
    public BashExe(ICliExecutionContext? context = null)
        : base(context)
    {
        this.Name = "bash";
        this.Windows = new[]
        {
            "%ProgramFiles%\\Git\\bin\\bash.exe",
            "%ProgramFiles%\\Git\\usr\\bin\\bash.exe",
            "%WINDIR%\\System32\\bash.exe",
        };
    }

    public static BashExe Default { get; } = new();

    protected override CliScriptCommand CreateScriptCommand(string script, CliStartInfo? cliStartInfo = null)
        => new BashScriptCommand(script, cliStartInfo);
}
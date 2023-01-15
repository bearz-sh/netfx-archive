using Bearz.Std;

namespace Ze.Cli.Bash;

public static class Sh
{
    public static ExecutableInfo Executable { get; } = new ExecutableInfo()
    {
        Name = "bash",
        Windows = new[]
        {
            "%ProgramFiles%\\Git\\usr\\bin\\sh.exe",
        },
    };

    public static CommandOutput Call(Action<CliArgsCommandBuilder> configure)
    {
        var builder = new CliArgsCommandBuilder();
        configure(builder);
        return Call(builder.Build());
    }

    public static CommandOutput Call(CliArgsCommand command)
        => Cli.Call(Executable, command);

    public static Task<CommandOutput> CallAsync(Action<CliArgsCommandBuilder> configure)
    {
        var builder = new CliArgsCommandBuilder();
        configure(builder);
        return CallAsync(builder.Build());
    }

    public static Task<CommandOutput> CallAsync(CliArgsCommand command)
        => Cli.CallAsync(Executable, command);

    public static CommandOutput Script(string script, CliStartInfo startInfo)
        => Script(new ShScriptCommand(script, startInfo));

    public static CommandOutput Script(ShScriptCommand command)
        => Cli.CallScript(Executable, command);

    public static Task<CommandOutput> ScriptAsync(
        string script,
        CliStartInfo startInfo,
        CancellationToken cancellationToken = default)
        => ScriptAsync(new ShScriptCommand(script, startInfo), cancellationToken);

    public static Task<CommandOutput> ScriptAsync(ShScriptCommand command, CancellationToken cancellationToken = default)
        => Cli.CallScriptAsync(Executable, command, cancellationToken);
}
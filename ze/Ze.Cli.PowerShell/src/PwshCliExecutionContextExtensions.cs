using Bearz.Cli.Execution;
using Bearz.Extensions.Cli;
using Bearz.Std;

namespace Ze.Cli.PowerShell;

public static class PwshCliExecutionContextExtensions
{
    public static CommandOutput RunPwsh(this ICliExecutionContext context, ICliCommand command)
        => Pwsh.Executable.Call(context, command);

    public static CommandOutput RunPwsh(this ICliExecutionContext context, CliArgsCommand command)
        => Pwsh.Executable.Call(context, command);

    public static CommandOutput RunPwsh(this ICliExecutionContext context, ICliCommandBuilder builder)
        => Pwsh.Executable.Call(context, builder.Build());

    public static Task<CommandOutput> RunPwshAsync(
        this ICliExecutionContext context,
        ICliCommand command,
        CancellationToken cancellationToken = default)
        => Pwsh.Executable.CallAsync(context, command, cancellationToken);

    public static Task<CommandOutput> RunPwshAsync(
        this ICliExecutionContext context,
        CliArgsCommand command,
        CancellationToken cancellationToken = default)
        => Pwsh.Executable.CallAsync(context, command, cancellationToken);

    public static Task<CommandOutput> RunPwshAsync(
        this ICliExecutionContext context,
        ICliCommandBuilder builder,
        CancellationToken cancellationToken = default)
        => Pwsh.Executable.CallAsync(context, builder.Build(), cancellationToken);

    public static CommandOutput RunPwshScript(this ICliExecutionContext context, CliScriptCommand command)
        => Pwsh.Executable.CallScript(context, command);

    public static CommandOutput RunPwshScript(this ICliExecutionContext context, string script, CliStartInfo? startInfo = null)
        => Pwsh.Executable.CallScript(context, new PwshScriptCommand(script, startInfo ?? new CliStartInfo()));

    public static Task<CommandOutput> RunPwshScriptAsync(this ICliExecutionContext context, CliScriptCommand command)
        => Pwsh.Executable.CallScriptAsync(context, command);

    public static Task<CommandOutput> RunPwshScriptAsync(this ICliExecutionContext context, string script, CliStartInfo? startInfo = null)
        => Pwsh.Executable.CallScriptAsync(context, new PwshScriptCommand(script, startInfo ?? new CliStartInfo()));
}
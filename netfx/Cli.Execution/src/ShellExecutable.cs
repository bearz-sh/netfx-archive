using Bearz.Std;

namespace Bearz.Cli.Execution;

public abstract class ShellExecutable : Executable
{
    protected ShellExecutable(ICliExecutionContext? context = null)
        : base(context)
    {
    }

    public CommandOutput OutputScript(string script, CliStartInfo? cliStartInfo = null)
    {
        var command = this.CreateScriptCommand(script, cliStartInfo);
        return this.OutputScript(command);
    }

    public CommandOutput OutputScript(CliScriptCommand command)
    {
        try
        {
            var cmd = this.CreateCommand(command);
            var output = cmd.Output();
            this.ApplyPostHooks(command, output);

            return output;
        }
        finally
        {
            if (File.Exists(command.TempFileName))
                File.Delete(command.TempFileName);
        }
    }

    public Task<CommandOutput> OutputScriptAsync(
        string script,
        CliStartInfo? cliStartInfo = null,
        CancellationToken cancellationToken = default)
    {
        var command = this.CreateScriptCommand(script, cliStartInfo);
        return this.OutputScriptAsync(command, cancellationToken);
    }

    public async Task<CommandOutput> OutputScriptAsync(
        CliScriptCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var cmd = this.CreateCommand(command);
            var output = await cmd.OutputAsync(cancellationToken)
                .ConfigureAwait(false);

            this.ApplyPostHooks(command, output);

            return output;
        }
        finally
        {
            if (File.Exists(command.TempFileName))
                File.Delete(command.TempFileName);
        }
    }

    protected abstract CliScriptCommand CreateScriptCommand(string script, CliStartInfo? cliStartInfo = null);
}
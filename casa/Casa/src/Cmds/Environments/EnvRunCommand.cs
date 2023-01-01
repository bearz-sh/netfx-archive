using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;
using Bearz.Std;

using Casa.Cmds.Utils;

using Command = System.CommandLine.Command;

namespace Casa.Cmds.Environments;

[CommandHandler(typeof(EnvRunCommandHandler))]
public class EnvRunCommand : Command
{
    public EnvRunCommand()
        : base("run", "Run a command using the given or current environment's variables and secrets")
    {
        this.AddOption(new Option<string?>(new[] { "casa-env", "ce" }, "The casa environment to use"));
        this.TreatUnmatchedTokensAsErrors = false;
        this.AddArgument(new Argument<string?>("command", result =>
        {
            result.OnlyTake(1);
            return result.Tokens[0].Value;
        }));

        this.AddArgument(new Argument<string[]?>("args", result =>
        {
            var list = new List<string>();
            foreach (var t in result.Tokens)
            {
                list.Add(t.Value);
            }

            return list.ToArray();
        }));
    }
}

public class EnvRunCommandHandler : ICommandHandler
{
    private readonly Domain.Settings settings;

    private readonly Domain.Environments environments;

    public EnvRunCommandHandler(Domain.Settings settings, Domain.Environments environments)
    {
        this.settings = settings;
        this.environments = environments;
    }

    public string? CasaEnv { get; set; }

    public string? Command { get; set; }

    public string[]? Args { get; set; }

    public int Invoke(InvocationContext context)
    {
        if (string.IsNullOrWhiteSpace(this.Command))
        {
            Console.Error.WriteLine("No command to run");
            return -1;
        }

        var env = EnvUtils.FindEnvironment(this.CasaEnv, this.settings, this.environments);
        if (env is null)
        {
            return -1;
        }

        foreach (var next in env.Variables)
        {
            Env.Set(next.Name, next.Value);
        }

        foreach (var next in env.Secrets)
        {
            Env.Set(next.Name, next.Value);
        }

        var cmd = Process.CreateCommand(
            this.Command,
            new CommandStartInfo()
            {
                Args = new CommandArgs(this.Args ?? Array.Empty<string>()),
                StdOut = Stdio.Inherit,
                StdErr = Stdio.Inherit,
            });
        var r = cmd.Output();
        return r.ExitCode;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult(this.Invoke(context));
}
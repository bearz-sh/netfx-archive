using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;
using Bearz.Std;

using Casa.Cmds.Utils;

using Command = System.CommandLine.Command;

namespace Casa.Cmds.Environments;

[CommandHandler(typeof(EnvEvalCommandHandler))]
public class EnvEvalCommand : Command
{
    public EnvEvalCommand()
        : base("eval", "Evaluates a string by substituting environment variables")
    {
        this.AddArgument(new Argument<string?>("template", "The string to evaluate and substitute environment variables"));
        this.AddOption(new Option<string?>(new[] { "casa-env", "ce" }, "The casa environment to use"));
        this.AddOption(new Option<string?>(new[] { "src", "s" }, "The source file to use. Must be Utf8 encoded"));
        this.AddOption(new Option<string?>(new[] { "dest", "d", "o" }, "The destination to save the file to."));
    }
}

public class EnvEvalCommandHandler : ICommandHandler
{
    private readonly Domain.Settings settings;

    private readonly Domain.Environments environments;

    public EnvEvalCommandHandler(Domain.Settings settings, Domain.Environments environments)
    {
        this.settings = settings;
        this.environments = environments;
    }

    public string? CasaEnv { get; set; }

    public string? Template { get; set; }

    public string? Src { get; set; }

    public string? Dest { get; set; }

    public int Invoke(InvocationContext context)
    {
        if (string.IsNullOrWhiteSpace(this.Template) && string.IsNullOrWhiteSpace(this.Src))
        {
            Console.Error.WriteLine("Either the [template] argument or the --src option must be specified");
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

        if (!string.IsNullOrWhiteSpace(this.Template))
        {
            var output = Env.Expand(this.Template);

            if (!string.IsNullOrWhiteSpace(this.Dest))
            {
                Fs.WriteTextFile(this.Dest, output);
                return 0;
            }

            Console.WriteLine(output);
            return 0;
        }

        if (!string.IsNullOrWhiteSpace(this.Src))
        {
            var output = Env.Expand(Fs.ReadTextFile(this.Src));

            if (!string.IsNullOrWhiteSpace(this.Dest))
            {
                Fs.WriteTextFile(this.Dest, output);
                return 0;
            }

            Console.WriteLine(output);
            return 0;
        }

        return -1;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult<int>(this.Invoke(context));
}
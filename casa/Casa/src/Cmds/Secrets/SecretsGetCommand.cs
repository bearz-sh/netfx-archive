using System.CommandLine;
using System.CommandLine.Invocation;
using System.Runtime.InteropServices;

using Bearz.Extensions.Hosting.CommandLine;

using Casa.Cmds.Utils;

namespace Casa.Cmds.Secrets;

[CommandHandler(typeof(SecretsGetCommandHandler))]
public class SecretsGetCommand : Command
{
    public SecretsGetCommand()
        : base("get", "Gets a secret from the secrets store")
    {
        var nameArg = new Argument<string>("name", "the name of the environment secret to get");
        nameArg.Arity = ArgumentArity.ExactlyOne;
        this.AddArgument(nameArg);
    }
}

public class SecretsGetCommandHandler : ICommandHandler
{
    private readonly Domain.Settings settings;

    private readonly Domain.Environments environments;

    public SecretsGetCommandHandler(Domain.Settings settings, Domain.Environments environments)
    {
        this.settings = settings;
        this.environments = environments;
    }

    public string? Env { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public int Invoke(InvocationContext context)
    {
        if (string.IsNullOrWhiteSpace(this.Name))
        {
            Console.Error.WriteLine("Name is required");
            return -1;
        }

        var env = EnvUtils.FindEnvironment(this.Env, this.settings, this.environments);
        if (env is null)
        {
            return -1;
        }

        Console.WriteLine(env.GetSecret(this.Name));
        return 0;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult(this.Invoke(context));
}
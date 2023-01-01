using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

using Casa.Cmds.Utils;

namespace Casa.Cmds.Secrets;

[CommandHandler(typeof(SecretsRemoveCommandHandler))]
public class SecretsRemoveCommand : Command
{
    public SecretsRemoveCommand()
        : base("remove", "Remove a secret from the environment store.")
    {
        var nameArg = new Argument<string>("name", "the name of the environment secret to remove");
        nameArg.Arity = ArgumentArity.ExactlyOne;
        this.AddArgument(nameArg);
    }
}

public class SecretsRemoveCommandHandler : ICommandHandler
{
    private readonly Domain.Settings settings;

    private readonly Domain.Environments environments;

    public SecretsRemoveCommandHandler(Domain.Settings settings, Domain.Environments environments)
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

        env.DeleteSecret(this.Name);
        return 0;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult(this.Invoke(context));
}
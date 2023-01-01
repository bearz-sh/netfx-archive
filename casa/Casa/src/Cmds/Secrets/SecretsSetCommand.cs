using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

using Casa.Cmds.Utils;

namespace Casa.Cmds.Secrets;

[CommandHandler(typeof(SecretsSetCommandHandler))]
public class SecretsSetCommand : Command
{
    public SecretsSetCommand()
        : base("set", "Sets a secret value")
    {
        var nameArg = new Argument<string>("name", "the value of the environment secret to set");
        nameArg.Arity = ArgumentArity.ExactlyOne;
        this.AddArgument(nameArg);

        var valueArg = new Argument<string>("value", "the value of the environment secret to set");
        valueArg.Arity = ArgumentArity.ExactlyOne;
        this.AddArgument(valueArg);
    }
}

public class SecretsSetCommandHandler : ICommandHandler
{
    private readonly Domain.Settings settings;

    private readonly Domain.Environments environments;

    public SecretsSetCommandHandler(Domain.Settings settings, Domain.Environments environments)
    {
        this.settings = settings;
        this.environments = environments;
    }

    public string? Env { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

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

        env.SetSecret(this.Name, this.Value);
        return 0;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult(this.Invoke(context));
}
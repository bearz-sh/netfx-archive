using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

using Casa.Cmds.Utils;
using Casa.Domain;

namespace Casa.Cmds.Environments.Var;

[CommandHandler(typeof(VarSetCommandHandler))]
public class VarSetCommand : Command
{
    public VarSetCommand()
        : base("set", "Sets an environment variable")
    {
        var nameArg = new Argument<string>("name", "the name of the environment variable to get");
        nameArg.Arity = ArgumentArity.ExactlyOne;
        this.AddArgument(nameArg);

        var valueArg = new Argument<string>("value", "the value of the environment variable to get");
        valueArg.Arity = ArgumentArity.ExactlyOne;
        this.AddArgument(valueArg);
    }
}

public class VarSetCommandHandler : ICommandHandler
{
    private readonly Settings settings;

    private readonly Domain.Environments environments;

    public VarSetCommandHandler(Settings settings, Domain.Environments environments)
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

        if (string.IsNullOrWhiteSpace(this.Value))
        {
            Console.Error.WriteLine("Value is required");
            return -1;
        }

        var env = EnvUtils.FindEnvironment(this.Env, this.settings, this.environments);
        if (env is null)
        {
            return -1;
        }

        env.SetVariable(this.Name, this.Value);

        return 0;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult(this.Invoke(context));
}
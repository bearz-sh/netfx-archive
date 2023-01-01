using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

using Casa.Cmds.Utils;
using Casa.Domain;

namespace Casa.Cmds.Environments.Var;

[CommandHandler(typeof(VarGetCommandHandler))]
public class VarGetCommand : Command
{
    public VarGetCommand()
        : base("get", "gets an environment variable")
    {
        var nameArg = new Argument<string>("name", "the name of the environment variable to get");
        nameArg.Arity = ArgumentArity.ExactlyOne;
        this.AddArgument(nameArg);
    }
}

public class VarGetCommandHandler : ICommandHandler
{
    private readonly Settings settings;

    private readonly Domain.Environments environments;

    public VarGetCommandHandler(Settings settings, Domain.Environments environments)
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

        Console.WriteLine(env.GetVariable(this.Name));
        return 0;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult(this.Invoke(context));
}
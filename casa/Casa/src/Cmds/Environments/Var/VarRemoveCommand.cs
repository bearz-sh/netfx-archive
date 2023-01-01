using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

using Casa.Cmds.Utils;
using Casa.Domain;

namespace Casa.Cmds.Environments.Var;

[CommandHandler(typeof(VarRemoveCommandHandler))]
public class VarRemoveCommand : Command
{
    public VarRemoveCommand()
        : base("remove", "Remove an environment variable")
    {
       this.AddAlias("rm");
    }
}

public class VarRemoveCommandHandler : ICommandHandler
{
    private readonly Settings settings;

    private readonly Domain.Environments environments;

    public VarRemoveCommandHandler(Settings settings, Domain.Environments environments)
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

        env.DeleteVariable(this.Name);
        return 0;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult(this.Invoke(context));
}
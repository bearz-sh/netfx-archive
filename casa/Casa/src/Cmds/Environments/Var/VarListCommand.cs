using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

using Casa.Cmds.Utils;
using Casa.Data.Model;
using Casa.Domain;

namespace Casa.Cmds.Environments.Var;

[CommandHandler(typeof(VarListCommandHandler))]
public class VarListCommand : Command
{
    public VarListCommand()
        : base("list", "List all environment variables")
    {
    }
}

public class VarListCommandHandler : ICommandHandler
{
    private readonly Settings settings;

    private readonly Domain.Environments environments;

    public VarListCommandHandler(Settings settings, Domain.Environments environments)
    {
        this.settings = settings;
        this.environments = environments;
    }

    public string? Env { get; set; } = string.Empty;

    public int Invoke(InvocationContext context)
    {
        var env = EnvUtils.FindEnvironment(this.Env, this.settings, this.environments);
        if (env is null)
        {
            return -1;
        }

        foreach (var variable in env.Variables)
        {
            Console.WriteLine($"{variable.Name}={variable.Value}");
        }

        return 0;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult(this.Invoke(context));
}
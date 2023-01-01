using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

using Casa.Cmds.Utils;

namespace Casa.Cmds.Secrets;

[CommandHandler(typeof(SecretsListCommandHandler))]
public class SecretsListCommand : Command
{
    public SecretsListCommand()
        : base("list", "lists the secrets available in the current or given environment")
    {
    }
}

public class SecretsListCommandHandler : ICommandHandler
{
    private readonly Domain.Settings settings;

    private readonly Domain.Environments environments;

    public SecretsListCommandHandler(Domain.Settings settings, Domain.Environments environments)
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

        foreach (var variable in env.Secrets)
        {
            Console.WriteLine($"{variable.Name}");
        }

        return 0;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult(this.Invoke(context));
}
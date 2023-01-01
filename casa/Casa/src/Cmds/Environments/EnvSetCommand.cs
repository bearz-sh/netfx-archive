using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

using Casa.Domain;

namespace Casa.Cmds.Environments;

[CommandHandler(typeof(EnvSetCommandHandler))]
public class EnvSetCommand : Command
{
    public EnvSetCommand()
        : base("set", "Set the deployment environment")
    {
        this.AddArgument(new Argument<string>("name", "The name of the environment variable"));
    }
}

public class EnvSetCommandHandler : ICommandHandler
{
    private readonly Settings settings;

    private readonly Casa.Domain.Environments environments;

    public EnvSetCommandHandler(Settings settings, Casa.Domain.Environments environments)
    {
        this.settings = settings;
        this.environments = environments;
    }

    public string Name { get; set; } = string.Empty;

    public int Invoke(InvocationContext context)
    {
        var name = this.Name.Trim();
        foreach (var c in name)
        {
            if (char.IsLetterOrDigit(c))
                continue;

            if (c is '_' or '-')
                continue;

            throw new InvalidOperationException($"Invalid character '{c}' in environment variable name '{name}'");
        }

        var env = this.environments.Get(name);
        if (env is null)
            this.environments.Create(name);

        this.settings.Set("env.name", name);
        return 0;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult(this.Invoke(context));
}
using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

namespace Casa.Cmds.Secrets;

[SubCommandHandler(typeof(SecretsCommandHandler))]
public class SecretsCommand : Command
{
    public SecretsCommand()
        : base("secrets", "Manages secrets for a the current or given environment")
    {
        this.AddGlobalOption(new Option<string?>(new[] { "env", "e" }, "The environment to use."));
        this.AddCommand(new SecretsGetCommand());
        this.AddCommand(new SecretsSetCommand());
        this.AddCommand(new SecretsRemoveCommand());
        this.AddCommand(new SecretsListCommand());
    }
}

public class SecretsCommandHandler : ICommandHandler
{
    public int Invoke(InvocationContext context)
    {
        throw new NotImplementedException();
    }

    public Task<int> InvokeAsync(InvocationContext context)
    {
        throw new NotImplementedException();
    }
}
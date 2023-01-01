using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

namespace Casa.Cmds.Secrets;

[CommandHandler(typeof(SecretsSetCommandHandler))]
public class SecretsSetCommand : Command
{
    public SecretsSetCommand()
        : base("set", "Sets a secret value")
    {
    }
}

public class SecretsSetCommandHandler : ICommandHandler
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
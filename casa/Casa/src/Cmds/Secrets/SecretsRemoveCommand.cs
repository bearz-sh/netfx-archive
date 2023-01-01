using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

namespace Casa.Cmds.Secrets;

[CommandHandler(typeof(SecretsRemoveCommandHandler))]
public class SecretsRemoveCommand : Command
{
    public SecretsRemoveCommand()
        : base("remove", "Remove a secret from the environment store.")
    {
    }
}

public class SecretsRemoveCommandHandler : ICommandHandler
{
    public int Invoke(InvocationContext context)
    {
        return 0;
    }

    public Task<int> InvokeAsync(InvocationContext context)
    {
        throw new NotImplementedException();
    }
}
using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

namespace Casa.Cmds.Secrets;

[CommandHandler(typeof(SecretsGetCommandHandler))]
public class SecretsGetCommand : Command
{
    public SecretsGetCommand()
        : base("get", "Gets a secret from the secrets store")
    {
    }
}

public class SecretsGetCommandHandler : ICommandHandler
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
using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

namespace Casa.Cmds.Stack.Network;

[CommandHandler(typeof(NetworkCreateCommandHandler))]
public class NetworkCreateCommand : Command
{
    public NetworkCreateCommand()
        : base("create", "create a network")
    {
    }
}

public class NetworkCreateCommandHandler : ICommandHandler
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
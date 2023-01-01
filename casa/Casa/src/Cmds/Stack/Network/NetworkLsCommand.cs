using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

namespace Casa.Cmds.Stack.Network;

[CommandHandler(typeof(NetworkLsCommandHandler))]
public class NetworkLsCommand : Command
{
    public NetworkLsCommand()
        : base("ls", "List networks")
    {
    }
}

public class NetworkLsCommandHandler : ICommandHandler
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
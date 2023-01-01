using System.CommandLine;
using System.CommandLine.Invocation;

namespace Casa.Cmds.Stack.Network;

public class NetworkRemoveCommand : Command
{
    public NetworkRemoveCommand()
        : base("remove", "Removes a network")
    {
        this.AddAlias("rm");
    }
}

public class NetworkRemoveCommandHandler : ICommandHandler
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
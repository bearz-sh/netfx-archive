using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

namespace Casa.Cmds.Stack.Network;

[CommandHandler(typeof(NetworkCommandHandler))]
public class NetworkCommand : Command
{
    public NetworkCommand()
        : base("network", "Manages docker networks")
    {
        this.AddCommand(new NetworkCreateCommand());
        this.AddCommand(new NetworkLsCommand());
        this.AddCommand(new NetworkRemoveCommand());
    }
}

public class NetworkCommandHandler : ICommandHandler
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
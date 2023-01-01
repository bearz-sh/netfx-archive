using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

namespace Casa.Cmds.Stack;

[CommandHandler(typeof(StackStartCommandHandler))]
public class StackStartCommand : Command
{
    public StackStartCommand()
        : base("start", "Starts the stack")
    {
    }
}

public class StackStartCommandHandler : ICommandHandler
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
using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

namespace Casa.Cmds.Stack;

[CommandHandler(typeof(StackCpCommandHandler))]
public class StackCpCommand : Command
{
    public StackCpCommand()
        : base("cp", "Copy files/folders between a service container and the local filesystem")
    {
    }
}

public class StackCpCommandHandler : ICommandHandler
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
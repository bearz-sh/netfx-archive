using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

namespace Casa.Cmds.Stack;

[CommandHandler(typeof(StackPsCommandHandler))]
public class StackPsCommand : Command
{
    public StackPsCommand()
        : base("ps", "List containers")
    {
    }
}

public class StackPsCommandHandler : ICommandHandler
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
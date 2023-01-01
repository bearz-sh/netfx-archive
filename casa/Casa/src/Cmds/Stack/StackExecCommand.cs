using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

namespace Casa.Cmds.Stack;

[CommandHandler(typeof(StackExecCommandHandler))]
public class StackExecCommand : Command
{
    public StackExecCommand()
        : base("exec", "Execute a command in the running container")
    {
    }
}

public class StackExecCommandHandler : ICommandHandler
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
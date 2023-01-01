using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

namespace Casa.Cmds.Stack;

[CommandHandler(typeof(StackUpCommandHandler))]
public class StackUpCommand : Command
{
    public StackUpCommand()
        : base("up", "runs the docker compose up command")
    {
    }
}

public class StackUpCommandHandler : ICommandHandler
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
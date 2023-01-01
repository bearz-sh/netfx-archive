using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

namespace Casa.Cmds.Stack;

[CommandHandler(typeof(StackDownCommandHandler))]
public class StackDownCommand : Command
{
    public StackDownCommand()
        : base("down", "runs the docker compose down command")
    {
    }
}

public class StackDownCommandHandler : ICommandHandler
{
    public int Invoke(InvocationContext context)
    {
        throw new NotImplementedException();
    }

    public Task<int> InvokeAsync(InvocationContext context)
    {
        return Task.FromResult(0);
    }
}
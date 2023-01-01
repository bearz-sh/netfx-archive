using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

using Casa.Cmds.Stack.Network;

namespace Casa.Cmds.Stack;

[SubCommandHandler(typeof(StackCommandHandler))]
public class StackCommand : Command
{
    public StackCommand()
        : base("stack", "Manages docker compose using the current or given environment")
    {
        this.AddGlobalOption(new Option<string?>(new[] { "env", "e" }, "The environment to use"));
        this.AddCommand(new StackCpCommand());
        this.AddCommand(new StackDownCommand());
        this.AddCommand(new StackExecCommand());
        this.AddCommand(new StackLogsCommand());
        this.AddCommand(new StackLsCommand());
        this.AddCommand(new StackPsCommand());
        this.AddCommand(new StackStartCommand());
        this.AddCommand(new StackStopCommand());
        this.AddCommand(new StackUpCommand());
        this.AddCommand(new NetworkCommand());
        this.AddCommand(new StackTplCommand());
    }
}

public class StackCommandHandler : ICommandHandler
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
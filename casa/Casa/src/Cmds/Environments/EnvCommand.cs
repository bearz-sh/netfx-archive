using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

namespace Casa.Cmds.Environments;

[SubCommandHandler(typeof(EnvCommandHandler))]
public class EnvCommand : Command
{
    public EnvCommand()
        : base("env", "Manage environment variables")
    {
        this.AddCommand(new SetCommand());
        this.AddCommand(new GetCommand());
        this.AddCommand(new ListCommand());
    }
}

public class EnvCommandHandler : ICommandHandler
{
    public int Invoke(InvocationContext context)
    {
        return 0;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult(0);
}
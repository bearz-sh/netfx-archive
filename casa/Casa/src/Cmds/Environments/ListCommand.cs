using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

namespace Casa.Cmds.Environments;

[CommandHandler(typeof(ListCommandHandler))]
public class ListCommand : Command
{
    public ListCommand()
        : base("list", "List all environments")
    {
    }
}

public class ListCommandHandler : ICommandHandler
{
    private readonly Casa.Domain.Environments environments;

    public ListCommandHandler(Casa.Domain.Environments environments)
    {
        this.environments = environments;
    }

    public int Invoke(InvocationContext context)
    {
        var set = this.environments.ListNames();
        foreach (var name in set)
        {
            Console.WriteLine(name);
        }

        return 0;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult(this.Invoke(context));
}
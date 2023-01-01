using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

namespace Casa.Cmds.Environments;

[CommandHandler(typeof(EnvRemoveCommandHandler))]
public class EnvRemoveCommand : Command
{
    public EnvRemoveCommand()
        : base("remove", "Removes the environment.")
    {
        this.AddArgument(new Argument<string>("name", "The name of the environment to remove."));
    }
}

public class EnvRemoveCommandHandler : ICommandHandler
{
    private readonly Casa.Domain.Environments environments;

    public EnvRemoveCommandHandler(Casa.Domain.Environments environments)
    {
        this.environments = environments;
    }

    public string Name { get; set; } = string.Empty;

    public int Invoke(InvocationContext context)
    {
        if (this.environments.Delete(this.Name))
            return 0;

        return -1;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult(this.Invoke(context));
}
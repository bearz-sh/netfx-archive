using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

using Casa.Domain;

namespace Casa.Cmds.Environments;

[CommandHandler(typeof(GetCommandHandler))]
public class GetCommand : Command
{
    public GetCommand()
        : base("get", "Gets this current environment.")
    {
    }
}

public class GetCommandHandler : ICommandHandler
{
    private readonly Settings settings;

    public GetCommandHandler(Settings settings)
    {
        this.settings = settings;
    }

    public int Invoke(InvocationContext context)
    {
        var env = this.settings.Get("env.name");
        if (string.IsNullOrWhiteSpace(env))
        {
            Console.WriteLine("No environment is set.");
            return -1;
        }

        Console.WriteLine(env);
        return 0;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult(this.Invoke(context));
}
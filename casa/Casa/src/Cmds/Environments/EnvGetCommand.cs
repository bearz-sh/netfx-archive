using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

using Casa.Domain;

namespace Casa.Cmds.Environments;

[CommandHandler(typeof(EnvGetCommandHandler))]
public class EnvGetCommand : Command
{
    public EnvGetCommand()
        : base("get", "Gets this current environment.")
    {
    }
}

public class EnvGetCommandHandler : ICommandHandler
{
    private readonly Settings settings;

    public EnvGetCommandHandler(Settings settings)
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
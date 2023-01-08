using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

using Casa.Domain;

namespace Casa.Cmds.Config;

[CommandHandler(typeof(ConfigGetCommandHandler))]
public class ConfigGetCommand : Command
{
    public ConfigGetCommand()
        : base("get", "Gets the value of a configuration setting.")
    {
        var args = new Argument<string>("name", "The name of the configuration setting.");
        args.Arity = ArgumentArity.ExactlyOne;
        this.AddArgument(args);
    }
}

public class ConfigGetCommandHandler : ICommandHandler
{
    private readonly Settings settings;

    public ConfigGetCommandHandler(Settings settings)
    {
        this.settings = settings;
    }

    public string Name { get; set; } = string.Empty;

    public int Invoke(InvocationContext context)
    {
        if (string.IsNullOrWhiteSpace(this.Name))
        {
            Console.Error.WriteLine("The name of the configuration setting to get is required.");
            return 1;
        }

        var value = this.settings.Get(this.Name);
        Console.WriteLine(value);
        return 0;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult(this.Invoke(context));
}
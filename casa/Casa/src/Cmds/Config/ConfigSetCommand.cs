using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

using Casa.Domain;

namespace Casa.Cmds.Config;

[CommandHandler(typeof(ConfigSetCommandHandler))]
public class ConfigSetCommand : Command
{
    public ConfigSetCommand()
        : base("set", "Sets a configuration value")
    {
        this.AddArgument(new Argument<string>("name", "The name of the config setting."));
        this.AddArgument(new Argument<string>("value", "The value of the config setting."));
    }
}

public class ConfigSetCommandHandler : ICommandHandler
{
    private readonly Settings settings;

    public ConfigSetCommandHandler(Settings settings)
    {
        this.settings = settings;
    }

    public string Name { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    public int Invoke(InvocationContext context)
    {
        if (string.IsNullOrWhiteSpace(this.Name))
        {
            Console.Error.WriteLine("The name of the configuration setting to get is required.");
            return 1;
        }

        this.settings.Set(this.Name, this.Value);

        return 0;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult(this.Invoke(context));
}
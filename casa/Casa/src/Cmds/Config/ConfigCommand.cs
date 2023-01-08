using System.CommandLine;
using System.CommandLine.Help;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

namespace Casa.Cmds.Config;

[SubCommandHandler(typeof(ConfigCommandHandler))]
public class ConfigCommand : Command
{
    public ConfigCommand()
        : base("config", "Manages configuration for the command line tool")
    {
        this.AddCommand(new ConfigGetCommand());
        this.AddCommand(new ConfigSetCommand());
    }
}

public class ConfigCommandHandler : ICommandHandler
{
    public int Invoke(InvocationContext context)
    {
        var ctx = new HelpContext(
            context.HelpBuilder,
            context.ParseResult.CommandResult.Command,
            Console.Out,
            context.ParseResult);
        context.HelpBuilder.Write(ctx);
        return 0;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult(this.Invoke(context));
}
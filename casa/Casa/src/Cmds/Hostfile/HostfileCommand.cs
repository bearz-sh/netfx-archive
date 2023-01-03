using System.CommandLine;
using System.CommandLine.Help;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

namespace Casa.Cmds.Hostfile;

[SubCommandHandler(typeof(HostfileCommandHandler))]
public class HostfileCommand : Command
{
    public HostfileCommand()
        : base("hostfile", "Provides subcommands to manage the hostfile")
    {
        this.AddCommand(new HostfileSetCommand());
        this.AddCommand(new HostfileListCommand());
        this.AddCommand(new HostfileRemoveCommand());
    }
}

public class HostfileCommandHandler : ICommandHandler
{
    public HostfileCommandHandler()
    {
    }

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
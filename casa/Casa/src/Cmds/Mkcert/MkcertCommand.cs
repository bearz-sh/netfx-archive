using System.CommandLine;
using System.CommandLine.Help;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

namespace Casa.Cmds.Mkcert;

[SubCommandHandler(typeof(MkcertCommandHandler))]
public class MkcertCommand : Command
{
    public MkcertCommand()
        : base("mkcert", "provides subcommands for mkcert. mkcert must be installed")
    {
        this.AddCommand(new MkcertInstallCommand());
        this.AddCommand(new MkcertNewCommand());
    }
}

public class MkcertCommandHandler : ICommandHandler
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
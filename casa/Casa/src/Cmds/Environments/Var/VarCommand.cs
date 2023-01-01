using System.CommandLine;
using System.CommandLine.Help;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

namespace Casa.Cmds.Environments.Var;

[CommandHandler(typeof(VarCommandHandler))]
public class VarCommand : Command
{
    public VarCommand()
        : base("var", "Manage environment variables")
    {
        this.AddCommand(new VarGetCommand());
        this.AddCommand(new VarSetCommand());
        this.AddCommand(new VarListCommand());
        this.AddCommand(new VarRemoveCommand());
    }
}

public class VarCommandHandler : ICommandHandler
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
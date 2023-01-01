using System.CommandLine;
using System.CommandLine.Help;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

using Casa.Cmds.Environments.Var;

namespace Casa.Cmds.Environments;

[SubCommandHandler(typeof(EnvCommandHandler))]
public class EnvCommand : Command
{
    public EnvCommand()
        : base("env", "Manage environment variables")
    {
        this.AddCommand(new EnvSetCommand());
        this.AddCommand(new EnvGetCommand());
        this.AddCommand(new EnvListCommand());
        this.AddCommand(new VarCommand());
        this.AddCommand(new EnvRemoveCommand());
        this.AddCommand(new EnvEvalCommand());
        this.AddCommand(new EnvRunCommand());
    }
}

public class EnvCommandHandler : ICommandHandler
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
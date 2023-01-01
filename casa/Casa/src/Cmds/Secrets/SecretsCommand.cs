using System.CommandLine;
using System.CommandLine.Help;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

namespace Casa.Cmds.Secrets;

[SubCommandHandler(typeof(SecretsCommandHandler))]
public class SecretsCommand : Command
{
    public SecretsCommand()
        : base("secrets", "Manages secrets for a the current or given environment")
    {
        this.AddGlobalOption(new Option<string?>(new[] { "env", "e" }, "The environment to use."));
        this.AddCommand(new SecretsGetCommand());
        this.AddCommand(new SecretsSetCommand());
        this.AddCommand(new SecretsRemoveCommand());
        this.AddCommand(new SecretsListCommand());
        this.AddCommand(new SecretsNewCommand());
    }
}

public class SecretsCommandHandler : ICommandHandler
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
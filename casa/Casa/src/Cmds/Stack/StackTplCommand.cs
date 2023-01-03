using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;

using Casa.Cmds.Utils;

namespace Casa.Cmds.Stack;

[CommandHandler(typeof(StackTplCommandHandler))]
public class StackTplCommand : Command
{
    public StackTplCommand()
        : base("tpl", "renders the templates for a given stack template")
    {
        this.AddArgument(new Argument<string>("stack", "The name of the stack to render"));
    }
}

public class StackTplCommandHandler : ICommandHandler
{
    private readonly Domain.Settings settings;

    private readonly Domain.Environments environments;

    public StackTplCommandHandler(Domain.Settings settings, Domain.Environments environments)
    {
        this.settings = settings;
        this.environments = environments;
    }

    public string? Env { get; set; }

    public string Stack { get; set; } = string.Empty;

    public int Invoke(InvocationContext context)
    {
        if (!Hbs.RenderDockerTemplates(this.Stack, this.Env, this.settings, this.environments))
            return -1;

        return 0;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult(this.Invoke(context));
}
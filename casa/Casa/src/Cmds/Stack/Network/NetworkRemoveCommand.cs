using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extra.Collections;
using Bearz.Std;

using Casa.Cmds.Utils;

using Command = System.CommandLine.Command;

namespace Casa.Cmds.Stack.Network;

public class NetworkRemoveCommand : Command
{
    public NetworkRemoveCommand()
        : base("remove", "Removes a network")
    {
        this.AddAlias("rm");
        this.AddArgument(new Argument<string>("network", "The network to remove"));
    }
}

public class NetworkRemoveCommandHandler : ICommandHandler
{
    private readonly Domain.Settings settings;

    public NetworkRemoveCommandHandler(Domain.Settings settings)
    {
        this.settings = settings;
    }

    public string Network { get; set; } = string.Empty;

    public int Invoke(InvocationContext context)
    {
        var args = new CommandArgs { "network", "remove", this.Network };
        var exe = Env.Get("CASA_COMPOSE_CLI") ?? this.settings.Get("compose.cli") ?? "docker";
        if (EnvUtils.UseSudoForDocker())
        {
            exe = "sudo";
            args.Unshift("docker");
        }

        var cmd = Process.CreateCommand(
            exe,
            new CommandStartInfo() { Args = args, StdOut = Stdio.Inherit, StdErr = Stdio.Inherit, });

        var r = cmd.Output();

        return r.ExitCode;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult(this.Invoke(context));
}
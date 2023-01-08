using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;
using Bearz.Extra.Collections;
using Bearz.Std;

using Casa.Cmds.Utils;

using Command = System.CommandLine.Command;

namespace Casa.Cmds.Stack.Network;

[CommandHandler(typeof(NetworkLsCommandHandler))]
public class NetworkLsCommand : Command
{
    public NetworkLsCommand()
        : base("ls", "List networks")
    {
        this.AddOption(new Option<string>(new[] { "filter", "f" }, "Filter output based on conditions provided"));
        this.AddOption(new Option<string>("format", "Pretty-print networks using a Go template"));
        this.AddOption(new Option<bool>("no-trunc", "Do not truncate output"));
        this.AddOption(new Option<bool>(new[] { "quiet", "q" }, "Only display numeric IDs"));
    }
}

public class NetworkLsCommandHandler : ICommandHandler
{
    private readonly Domain.Settings settings;

    public NetworkLsCommandHandler(Domain.Settings settings)
    {
        this.settings = settings;
    }

    public string? Filter { get; set; }

    public string? Format { get; set; } = string.Empty;

    public bool NoTrunc { get; set; }

    public bool Quiet { get; set; }

    public int Invoke(InvocationContext context)
    {
        var args = new CommandArgs { "network", "ls" };

        if (this.NoTrunc)
            args.Add("--no-trunc");

        if (this.Quiet)
            args.Add("--quiet");

        if (!string.IsNullOrWhiteSpace(this.Format))
        {
            args.Add("--format");
            args.Add(this.Format);
        }

        if (!string.IsNullOrWhiteSpace(this.Filter))
        {
            args.Add("--filter");
            args.Add(this.Filter);
        }

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
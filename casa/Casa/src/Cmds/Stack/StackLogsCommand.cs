using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;
using Bearz.Extra.Collections;
using Bearz.Std;

using Casa.Cmds.Utils;

using Command = System.CommandLine.Command;
using Path = System.IO.Path;

namespace Casa.Cmds.Stack;

[CommandHandler(typeof(StackLogsCommandHandler))]
public class StackLogsCommand : Command
{
    public StackLogsCommand()
        : base("logs", "View output from containers")
    {
        var arg = new Argument<string>("container", "The command to run")
        {
            Arity = ArgumentArity.ExactlyOne,
        };

        this.AddArgument(arg);
        this.AddOption(new Option<bool>("details", "Show extra details provided to logs"));
        this.AddOption(new Option<bool>(new[] { "follow", "f" }, "Follow log output"));
        this.AddOption(new Option<string?>("since", "Shows timestamps."));
        this.AddOption(new Option<string?>(new[] { "tail", "t" }, "Number of lines to show from the end of the logs."));
        this.AddOption(new Option<string?>("until", "Shows timestamps."));
    }
}

public class StackLogsCommandHandler : ICommandHandler
{
    private readonly Domain.Settings settings;

    public StackLogsCommandHandler(Domain.Settings settings)
    {
        this.settings = settings;
    }

    public string Container { get; set; } = string.Empty;

    public bool Details { get; set; }

    public bool Follow { get; set; }

    public string? Tail { get; set; }

    public bool Timestamps { get; set; }

    public string? Util { get; set; }

    public int Invoke(InvocationContext context)
    {
        if (string.IsNullOrWhiteSpace(this.Container))
        {
            Console.Error.WriteLine("Missing the container name");
            return -1;
        }

        var exe = Env.Get("CASA_COMPOSE_CLI") ?? this.settings.Get("compose.cli") ?? "docker";
        var args = new CommandArgs { "compose", "logs" };

        if (this.Details)
            args.Add("--details");

        if (this.Timestamps)
            args.Add("--timestamps");

        if (this.Follow)
            args.Add("--follow");

        if (!string.IsNullOrWhiteSpace(this.Tail))
            args.Add("--tail", this.Tail);

        if (!string.IsNullOrWhiteSpace(this.Util))
            args.Add("--util", this.Util);

        args.Add(this.Container);

        if (EnvUtils.UseSudoForDocker())
        {
            exe = "sudo";
            args.Unshift("docker");
        }

        var cmd = Process.CreateCommand(exe, new CommandStartInfo()
        {
            Args = args,
            StdOut = Stdio.Inherit,
            StdErr = Stdio.Inherit,
        });

        var r = cmd.Output();

        return r.ExitCode;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult(this.Invoke(context));
}
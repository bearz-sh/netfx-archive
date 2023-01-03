using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;
using Bearz.Extra.Collections;
using Bearz.Std;

using Casa.Cmds.Utils;

using Command = System.CommandLine.Command;

namespace Casa.Cmds.Stack.Network;

[CommandHandler(typeof(NetworkDefaultCommandHandler))]
public class NetworkDefaultCommand : Command
{
    public NetworkDefaultCommand()
        : base("default", "Creates the default docker-vnet network")
    {
    }
}

public class NetworkDefaultCommandHandler : ICommandHandler
{
    public int Invoke(InvocationContext context)
    {
        var args = new CommandArgs()
        {
            "network",
            "ls",
            "--format",
            "{{ .Name }}",
        };

        var exe = "docker";
        if (EnvUtils.UseSudoForDocker())
        {
            exe = "sudo";
            args.Unshift("docker");
        }

        var cmd = Process.CreateCommand(
            exe,
            new CommandStartInfo() { Args = args, StdOut = Stdio.Piped, StdErr = Stdio.Piped, });

        var r = cmd.Output();
        if (r.ExitCode != 0)
        {
            Console.WriteLine(args);
            Console.Error.WriteLine("Failed to list docker networks");
            return r.ExitCode;
        }

        var networks = r.StdOut.ToList();
        if (!networks.Contains("docker-vnet"))
        {
            args = new CommandArgs()
            {
                "network",
                "create",
                "--driver",
                "bridge",
                "--gateway",
                "176.16.0.1",
                "--subnet",
                "176.16.0.0/21",
                "--ip-range",
                "176.16.0.0/21",
                "docker-vnet",
            };

            exe = "docker";
            if (EnvUtils.UseSudoForDocker())
            {
                exe = "sudo";
                args.Unshift("docker");
            }

            cmd = Process.CreateCommand(
                exe,
                new CommandStartInfo() { Args = args, StdOut = Stdio.Inherit, StdErr = Stdio.Inherit, });

            r = cmd.Output();
            return r.ExitCode;
        }

        Console.WriteLine("docker-vnet already exists");
        return 0;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult(this.Invoke(context));
}
using System.CommandLine;
using System.CommandLine.Invocation;

using Bearz.Extensions.Hosting.CommandLine;
using Bearz.Extra.Collections;
using Bearz.Std;

using Casa.Cmds.Utils;

using Command = System.CommandLine.Command;

namespace Casa.Cmds.Stack.Network;

[CommandHandler(typeof(NetworkCreateCommandHandler))]
public class NetworkCreateCommand : Command
{
    public NetworkCreateCommand()
        : base("create", "create a network")
    {
        this.AddArgument(new Argument<string>("network", "the name of the network"));
        this.AddOption(new Option<bool>("config-only", "only create the config file"));
        this.AddOption(new Option<bool>("ipv6", "enable ipv6"));
        this.AddOption(new Option<bool>("internal", "internal network"));
        this.AddOption(new Option<bool>("attachable", "attachable network"));
        this.AddOption(new Option<bool>("ingress", "ingress network"));
        this.AddOption(new Option<string?>("ipam-driver", "ipam driver"));
        this.AddOption(new Option<string[]?>("ipam-opt", "ipam options"));
        this.AddOption(new Option<string[]?>("gateway", "gateway"));
        this.AddOption(new Option<string[]?>("subnet", "subnet"));
        this.AddOption(new Option<string[]?>("ip-range", "The ip-range"));
        this.AddOption(new Option<string[]?>(new[] { "opt", "o" }, "The driver options"));
    }
}

public class NetworkCreateCommandHandler : ICommandHandler
{
    private readonly Domain.Settings settings;

    public NetworkCreateCommandHandler(Domain.Settings settings)
    {
        this.settings = settings;
    }

    public string Network { get; set; } = string.Empty;

    public bool ConfigOnly { get; set; }

    public string? Driver { get; set; }

    public string[]? Gateway { get; set; }

    public bool Ingress { get; set; }

    public bool Internal { get; set; }

    public string[]? IpRange { get; set; }

    public string? IpamDriver { get; set; }

    public string[]? IpamOpt { get; set; }

    public string[]? Opt { get; set; }

    public bool IpV6 { get; set; }

    public string[]? Subnet { get; set; }

    public string[]? Label { get; set; }

    public int Invoke(InvocationContext context)
    {
        var args = new CommandArgs { "network", "create" };

        if (this.Ingress)
            args.Add("--ingress");

        if (this.IpV6)
            args.Add("--ipv6");

        if (this.ConfigOnly)
            args.Add("--config-only");

        if (this.IpRange != null)
        {
            foreach (var r in this.IpRange)
            {
                args.Add("--ip-range");
                args.Add(r);
            }
        }

        if (this.Gateway != null)
        {
            foreach (var r in this.Gateway)
            {
                args.Add("--gateway");
                args.Add(r);
            }
        }

        if (this.Subnet != null)
        {
            foreach (var r in this.Subnet)
            {
                args.Add("--subnet");
                args.Add(r);
            }
        }

        if (!string.IsNullOrWhiteSpace(this.Driver))
        {
            args.Add("--driver");
            args.Add(this.Driver);
        }

        if (this.Opt != null)
        {
            foreach (var r in this.Opt)
            {
                args.Add("--opt");
                args.Add(r);
            }
        }

        if (!string.IsNullOrWhiteSpace(this.IpamDriver))
        {
            args.Add("--ipam-driver");
            args.Add(this.IpamDriver);
        }

        if (this.IpamOpt != null)
        {
            foreach (var r in this.IpamOpt)
            {
                args.Add("--ipam-opt");
                args.Add(r);
            }
        }

        if (this.Label is not null)
        {
            foreach (var l in this.Label)
            {
                args.Add("--label");
                args.Add(l);
            }
        }

        args.Add(this.Network);
        var exe = Env.Get("CASA_COMPOSE_CLI") ?? this.settings.Get("compose.cli") ?? "docker";

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

        var result = cmd.Output();
        return result.ExitCode;
    }

    public Task<int> InvokeAsync(InvocationContext context)
        => Task.FromResult(this.Invoke(context));
}
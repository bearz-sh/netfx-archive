using Bearz.Cli.Execution;

using FluentBuilder;

using Ze.Cli;

namespace Ze.Cli.Docker;

[AutoGenerateBuilder]
public class DockerNetworkCreateCmd : DockerCmd
{
    public DockerNetworkCreateCmd(CliStartInfo? startInfo = null)
        : base("network create", startInfo)
    {
        this.CliArgumentsNames = new[] { "Network" };
    }

    public string Network { get; set; } = string.Empty;

    public NetworkDriver Driver { get; set; } = NetworkDriver.Bridge;

    public bool ConfigOnly { get; set; }

    public bool Internal { get; set; }

    public bool Attachable { get; set; }

    public bool Ipv6 { get; set; }

    public string? Scope { get; set; }

    public IReadOnlyCollection<string>? Gateway { get; set; }

    public IReadOnlyCollection<string>? IpRange { get; set; }

    public IReadOnlyCollection<string>? AuxAddress { get; set; }

    public IReadOnlyCollection<string>? Subnet { get; set; }
}
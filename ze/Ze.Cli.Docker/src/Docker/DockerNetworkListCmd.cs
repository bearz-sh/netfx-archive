using Bearz.Cli.Execution;

namespace Ze.Cli.Docker;

[AutoGenerateBuilder]
public class DockerNetworkListCmd : DockerCmd
{
    public DockerNetworkListCmd(CliStartInfo? startInfo = null)
        : base("network ls", startInfo)
    {
    }

    public string? Filter { get; set; }

    public string? Format { get; set; }

    public bool Quiet { get; set; }

    public bool NoTrunc { get; set; }
}
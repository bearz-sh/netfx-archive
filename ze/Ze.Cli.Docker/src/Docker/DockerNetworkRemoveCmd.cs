using Bearz.Cli.Execution;

using FluentBuilder;

namespace Ze.Cli.Docker;

[AutoGenerateBuilder]
public class DockerNetworkRemoveCmd : DockerCmd
{
    public DockerNetworkRemoveCmd(CliStartInfo? cliStartInfo = null)
        : base("network remove", cliStartInfo)
    {
        this.CliArgumentsNames = new[] { "Networks " };
    }

    public IReadOnlyCollection<string> Networks { get; set; } = Array.Empty<string>();
}
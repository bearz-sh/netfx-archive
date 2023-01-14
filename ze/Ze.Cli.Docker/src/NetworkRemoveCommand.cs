using FluentBuilder;

namespace Ze.Cli.Docker;

[AutoGenerateBuilder]
public class NetworkRemoveCommand : DockerCommand
{
    public NetworkRemoveCommand()
        : base("network remove")
    {
        this.CliArgumentsNames = new[] { "Networks " };
    }

    public IReadOnlyCollection<string> Networks { get; set; } = Array.Empty<string>();
}
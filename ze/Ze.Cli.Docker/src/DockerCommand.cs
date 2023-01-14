namespace Ze.Cli.Docker;

public abstract class DockerCommand : CliCommand
{
    protected DockerCommand(string? commandName)
        : base(commandName)
    {
    }
}
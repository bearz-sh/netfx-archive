using Bearz.Cli.Execution;

namespace Ze.Cli.Docker;

public abstract class DockerCmd : CliCommand
{
    protected DockerCmd(string? commandName, CliStartInfo? cliStartInfo = null)
        : base(commandName, cliStartInfo)
    {
    }
}
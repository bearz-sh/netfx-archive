using Bearz.Cli.Execution;

namespace Ze.Cli.Docker;

public class DockerExe : Executable
{
    public DockerExe(ICliExecutionContext? context = null)
        : base(context)
    {
        this.Name = "docker";
        this.Windows = new[] { "%Program Files%\\Docker\\Docker\\resources\\bin\\docker.exe", };
        this.Linux = new[] { "usr/bin/docker" };
    }

    public static DockerExe Default { get; } = new DockerExe();
}
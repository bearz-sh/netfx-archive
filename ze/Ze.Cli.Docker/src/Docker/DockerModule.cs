using Ze.Cli.Docker;

// ReSharper disable once CheckNamespace
namespace Ze.Cli;

public static class DockerModule
{
    public static DockerExe Docker { get; } = DockerExe.Default;

    public static NerdCtlExe NerdCtl { get; } = NerdCtlExe.Default;
}
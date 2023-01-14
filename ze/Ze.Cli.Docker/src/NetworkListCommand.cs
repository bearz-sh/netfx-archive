namespace Ze.Cli.Docker;

[AutoGenerateBuilder]
public class NetworkListCommand : DockerCommand
{
    public NetworkListCommand()
        : base("network ls")
    {
    }

    public string? Filter { get; set; }

    public string? Format { get; set; }

    public bool Quiet { get; set; }

    public bool NoTrunc { get; set; }
}
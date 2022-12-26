namespace Std.OS;

public class CommandOutput
{
    public int Id { get; internal set; }

    public int ExitCode { get; internal set; }

    public DateTime? StartedAt { get; internal set; }

    public DateTime? ExitedAt { get; internal set; }

    public IReadOnlyList<string> StdOut { get; internal set; } = Array.Empty<string>();

    public IReadOnlyList<string> StdErr { get; internal set; } = Array.Empty<string>();
}
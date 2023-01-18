using Bearz.Diagnostics;

namespace Bearz.Std;

public class CommandOutput
{
    public int Id { get; internal set; }

    public string Executable { get; set; } = string.Empty;

    public int ExitCode { get; internal set; }

    public DateTime? StartedAt { get; internal set; }

    public DateTime? ExitedAt { get; internal set; }

    public IReadOnlyList<string> StdOut { get; internal set; } = Array.Empty<string>();

    public IReadOnlyList<string> StdErr { get; internal set; } = Array.Empty<string>();

    public CommandOutput ValidateExitCode(Func<int, bool>? validate)
    {
        validate ??= code => code == 0;
        if (!validate(this.ExitCode))
            throw new ProcessException(this.ExitCode, this.Executable);

        return this;
    }
}
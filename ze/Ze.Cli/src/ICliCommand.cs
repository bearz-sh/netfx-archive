using Bearz.Std;

namespace Ze.Cli;

public interface ICliCommand
{
    string? CommandName { get; }

    CliStartInfo CliStartInfo { get; }

    CommandStartInfo Build();
}
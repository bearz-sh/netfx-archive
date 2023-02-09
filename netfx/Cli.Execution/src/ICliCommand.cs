using Bearz.Std;

namespace Bearz.Cli.Execution;

public interface ICliCommand
{
    string? CommandName { get; }

    CliStartInfo CliStartInfo { get; }

    CommandStartInfo Build();
}
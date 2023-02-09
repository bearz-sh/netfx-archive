using Bearz.Std;

namespace Bearz.Cli.Execution;

public interface IPostCliHook
{
    void Next(string executable, ICliCommand command, CommandOutput output);
}
using Bearz.Std;

namespace Ze.Cli;

public interface IPostCliHook
{
    void Next(string executable, ICliCommand command, CommandOutput output);
}
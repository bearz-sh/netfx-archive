namespace Bearz.Cli.Execution;

public interface IPreCliHook
{
    void Next(Executable executable, ICliCommand command);
}
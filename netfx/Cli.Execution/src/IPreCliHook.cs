namespace Bearz.Cli.Execution;

public interface IPreCliHook
{
    void Next(ExecutableInfo executable, ICliCommand command);
}
namespace Ze.Cli;

public interface IPreCliHook
{
    void Next(ExecutableInfo executable, CliCommand command);
}
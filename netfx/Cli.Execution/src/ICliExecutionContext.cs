using Bearz.Virtual;

namespace Bearz.Cli.Execution;

public interface ICliExecutionContext
{
    IServiceProvider Services { get; }

    IEnvironment Env { get; }

    IProcess Process { get; }

    IFileSystem Fs { get; }

    IPath Path { get; }
}
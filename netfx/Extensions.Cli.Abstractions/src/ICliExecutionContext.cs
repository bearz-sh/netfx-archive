using Bearz.Extensions.Console;
using Bearz.Virtual.Environment;
using Bearz.Virtual.FileSystem;

using Microsoft.Extensions.Logging;

namespace Bearz.Extensions.Cli;

public interface ICliExecutionContext
{
    IEnvironment Env { get; }

    IFileSystem Fs { get; }

    IPath Path { get; }

    IServiceProvider Services { get; }

    IConsoleHost ConsoleHost { get; }

    IProcess Process { get; }
}
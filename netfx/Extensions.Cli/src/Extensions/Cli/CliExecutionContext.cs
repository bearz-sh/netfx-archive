using Bearz.Extensions.Console;
using Bearz.Virtual.Environment;
using Bearz.Virtual.FileSystem;

using Microsoft.Extensions.DependencyInjection;

namespace Bearz.Extensions.Cli;

public class CliExecutionContext : ICliExecutionContext
{
    public CliExecutionContext(IServiceProvider serviceProvider)
    {
        this.Services = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        this.Env = serviceProvider.GetService<IEnvironment>() ?? new VirtualEnvironment();
        this.Path = serviceProvider.GetService<IPath>() ?? new VirtualPath(this.Env);
        this.Fs = serviceProvider.GetService<IFileSystem>() ?? new VirtualFileSystem(this.Path);
        this.ConsoleHost = serviceProvider.GetService<IConsoleHost>() ?? new ConsoleHost();
        this.Process = serviceProvider.GetService<IProcess>() ?? new VirtualProcess(this.Env);
    }

    public IEnvironment Env { get; }

    public IFileSystem Fs { get; }

    public IPath Path { get; }

    public IServiceProvider Services { get; }

    public IConsoleHost ConsoleHost { get; }

    public IProcess Process { get; }
}
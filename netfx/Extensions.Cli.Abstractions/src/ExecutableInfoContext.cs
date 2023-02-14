using Bearz.Cli.Execution;

namespace Bearz.Extensions.Cli;

public class ExecutableInfoContext : ExecutableInfo
{
    public ExecutableInfoContext(ExecutableInfo info, ICliExecutionContext context)
    {
        this.Context = context ?? throw new ArgumentNullException(nameof(context));
        this.Linux = info.Linux;
        this.Windows = info.Windows;
        this.MacOs = info.MacOs;
        this.Name = info.Name;
        this.Location = info.Location;
    }

    public ICliExecutionContext Context { get; }
}
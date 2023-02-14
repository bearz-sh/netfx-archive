using Bearz.Virtual;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Ze.Tasks;

public interface IExecutionContext
{
    IServiceProvider Services { get; }

    IEnvironment Env { get; }

    IPath Path { get; }

    IFileSystem Fs { get; }

    IProcess Process { get; }

    ILogger Log { get; }

    IConfiguration Config { get; }

    IMessageBus Bus { get; }
}
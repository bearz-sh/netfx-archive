using System;
using System.Linq;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Ze.Virtual.Environment;
using Ze.Virtual.FileSystem;

namespace Ze.Tasks;

public interface IExecutionContext
{
    IEnvironment Env { get; }

    IProcess Process { get; }

    IPath Path { get; }

    IFileSystem Fs { get; }

    IServiceProvider Services { get; }

    ILogger Log { get; }

    IConfiguration Config { get; }

    IMessageBus Bus { get; }
}
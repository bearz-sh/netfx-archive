using Bearz.Extensions.Cli;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Ze.Tasks;

public interface IExecutionContext : ICliExecutionContext
{
    ILogger Log { get; }

    IConfiguration Config { get; }

    IMessageBus Bus { get; }
}
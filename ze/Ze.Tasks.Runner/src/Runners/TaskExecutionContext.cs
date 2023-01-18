using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ze.Tasks.Internal;

public class TaskExecutionContext : ExecutionContext, ITaskExecutionContext
{
    public TaskExecutionContext(string id, IServiceProvider services)
        : base(services)
    {
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        this.Log = loggerFactory.CreateLogger(id);
        this.Outputs = new Outputs(id, "tasks", this.Env.SecretMasker);
    }

    public TaskExecutionContext(string id, IExecutionContext context)
        : base(context)
    {
        var loggerFactory = context.Services.GetRequiredService<ILoggerFactory>();
        this.Log = loggerFactory.CreateLogger(id);

        if (context is IActionExecutionContext tec)
        {
            this.Outputs = new Outputs(id, "tasks", this.Env.SecretMasker, tec.Outputs);
        }
        else
        {
            this.Outputs = new Outputs(id, "tasks", this.Env.SecretMasker);
        }
    }

    public IOutputs Outputs { get; }
}
using Microsoft.Extensions.Logging;

using Ze.Tasks.Messages;

namespace Ze.Tasks;

public class FuncTask : ZeTask
{
    private readonly Func<ITaskExecutionContext, Task> action;

    public FuncTask(string name, Func<ITaskExecutionContext, Task> action)
        : base(name)
    {
        this.action = action;
    }

    public FuncTask(string name, string id, Func<ITaskExecutionContext, Task> action)
        : base(name, id)
    {
        this.action = action;
    }

    public override async Task<TaskResult> RunAsync(ITaskExecutionContext context, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            return TaskResult.Cancelled();

        try
        {
            await this.action(context);
        }
        catch (Exception e)
        {
            context.Bus.QueueMessage(new TaskErrorMessage(e, this));
            return TaskResult.Failed();
        }

        return TaskResult.Completed();
    }
}
using Microsoft.Extensions.Logging;

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

    public override async Task<TaskStatus> RunAsync(ITaskExecutionContext context, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            return TaskStatus.Cancelled;

        try
        {
            await this.action(context);
        }
        catch (Exception e)
        {
            context.Log.LogError("Unhandled exception in task {TaskId}: {Exception}", this.Id, e);
            return TaskStatus.Failed;
        }

        return TaskStatus.Completed;
    }
}
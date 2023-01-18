using Microsoft.Extensions.Logging;

namespace Ze.Tasks;

public sealed class ActionTask : ZeTask
{
    private readonly Action<ITaskExecutionContext> action;

    public ActionTask(string name, Action action)
        : base(name)
    {
        this.action = _ => action();
    }

    public ActionTask(string name, string id, Action action)
        : base(name, id)
    {
        this.action = _ => action();
    }

    public ActionTask(string name, Action<ITaskExecutionContext> action)
        : base(name)
    {
        this.action = action;
    }

    public ActionTask(string name, string id, Action<ITaskExecutionContext> action)
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
            var task = new Task(() => this.action(context), cancellationToken);
            task.Start();
            await task;
        }
        catch (Exception e)
        {
            context.Log.LogError("Unhandled exception in task {TaskId}: {Exception}", this.Id, e);
            return TaskResult.Failed();
        }

        return TaskResult.Completed();
    }
}
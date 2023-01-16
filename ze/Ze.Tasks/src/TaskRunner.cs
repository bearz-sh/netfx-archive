using System.Text;

using Bearz.Extra.Strings;
using Bearz.Text;

using Microsoft.Extensions.Logging;

using Ze.Tasks.Internal;

namespace Ze.Tasks;

public class TaskRunner
{
    public async Task<TaskStatus> RunAsync(
        string target,
        TaskCollection tasks,
        IExecutionContext context,
        CancellationToken cancellationToken = default)
    {
        var sb = StringBuilderCache.Acquire();
        foreach (var c in target)
        {
            if (char.IsLetterOrDigit(c))
            {
                if (char.IsUpper(c))
                {
                    sb.Append(char.ToLower(c));
                    continue;
                }

                sb.Append(c);
                continue;
            }

            sb.Append("_");
        }

        var t = StringBuilderCache.GetStringAndRelease(sb);

        var rootTask = tasks.FirstOrDefault(o => o.Id.EqualsIgnoreCase(t) || o.Name.EqualsIgnoreCase(t));
        if (rootTask is null)
        {
            context.Log.LogError("Task not found: {0}", t);
            return TaskStatus.Failed;
        }

        if (rootTask.Dependencies.Count == 0)
        {
            var ct = cancellationToken;
            if (rootTask.Timeout > 0)
            {
                var ctc = new CancellationTokenSource(rootTask.Timeout);
                var cts = CancellationTokenSource.CreateLinkedTokenSource(ctc.Token, cancellationToken);
                ct = cts.Token;
            }

            TaskStatus status = TaskStatus.Completed;

            try
            {
                var ctx = new TaskExecutionContext(rootTask.Id, context);
                status = await rootTask.RunAsync(ctx, ct);
            }
            catch (Exception ex)
            {
                context.Log.LogError("Task {TaskId} failed with exception: {Exception}", rootTask.Id, ex);
                return TaskStatus.Failed;
            }

            return status;
        }
        else
        {
            tasks.CheckForCircularDependencies();
            tasks.CheckForMissingDependencies();

            var list = new List<ITask>();
            Collect(list, rootTask, tasks);
            return await this.RunAsync(tasks, context, cancellationToken)
                .ConfigureAwait(false);
        }
    }

    public async Task<TaskStatus> RunAsync(
        IReadOnlyList<ITask> tasks,
        IExecutionContext context,
        CancellationToken cancellationToken = default)
    {
        IExecutionContext previousContext = context;
        var failed = false;
        foreach (var task in tasks)
        {
            var id = task.Id;
            var ctx = new TaskExecutionContext(id, previousContext);
            previousContext = ctx;

            CancellationToken ct = cancellationToken;

            if (cancellationToken.IsCancellationRequested)
            {
                continue;
            }

            if (failed && !task.ContinueOnError)
            {
                continue;
            }

            if (task.Timeout > 0)
            {
                var ctc = new CancellationTokenSource(task.Timeout);
                var cts = CancellationTokenSource.CreateLinkedTokenSource(ctc.Token, cancellationToken);
                ct = cts.Token;
            }

            TaskStatus status = TaskStatus.Completed;

            try
            {
                status = await task.RunAsync(ctx, ct);
            }
            catch (Exception ex)
            {
                status = TaskStatus.Failed;
                ctx.Log.LogError("Task {TaskId} failed with exception: {Exception}", id, ex);
            }

            if (status == TaskStatus.Failed)
            {
                failed = true;
            }
        }

        if (failed)
            return TaskStatus.Failed;

        return cancellationToken.IsCancellationRequested ? TaskStatus.Cancelled : TaskStatus.Completed;
    }

    private static void Collect(List<ITask> list, ITask task, TaskCollection tasks)
    {
        foreach (var dependency in task.Dependencies)
        {
            var depJob = tasks.FirstOrDefault(o => o.Name.EqualsIgnoreCase(dependency) || o.Id.EqualsIgnoreCase(dependency));
            if (depJob is null)
            {
                throw new InvalidOperationException($"Task '{task.Name}' depends on '{dependency}' which is not defined");
            }

            Collect(list, depJob, tasks);
        }

        if (!list.Any(o => o.Id.EqualsIgnoreCase(task.Id)))
        {
            list.Add(task);
        }
    }
}
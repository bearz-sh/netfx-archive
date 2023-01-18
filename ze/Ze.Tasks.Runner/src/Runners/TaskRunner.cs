using System.Text;

using Bearz.Extra.Strings;
using Bearz.Std;
using Bearz.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Ze.Tasks.Internal;
using Ze.Tasks.Messages;

namespace Ze.Tasks.Runners;

public class TaskRunner : ITaskRunner
{
    private readonly IServiceProvider services;

    public TaskRunner(IServiceProvider services)
    {
        this.services = services;
    }

    public async Task<TaskRunnerResult> RunAsync(
        IDependencyCollection<ITask> tasks,
        ITaskRunOptions? options = null,
        IExecutionContext? context = null,
        CancellationToken cancellationToken = default)
    {
        options ??= new TaskRunOptions();
        IMessageBus bus = context?.Bus ?? this.services.GetRequiredService<IMessageBus>();
        var set = new List<ITask>();

        if (options.Targets.Count == 0)
        {
            bus.QueueMessage(new ErrorMessage("Missing task name."));
            return TaskRunnerResult.Failed();
        }

        try
        {
            if (options.Targets.Count == 1)
            {
                var taskName = options.Targets[0];
                var rootTask = tasks[taskName];
                if (rootTask is null)
                {
                    bus.QueueMessage(new ErrorMessage($"Unable to find a task with the name '{taskName}'."));
                    return TaskRunnerResult.Failed();
                }

                if (rootTask.Dependencies.Count == 0 || options.SkipDependencies)
                {
                    var ctx = context != null
                        ? new TaskExecutionContext(rootTask.Id, context)
                        : new TaskExecutionContext(rootTask.Id, this.services);

                    var ct = cancellationToken;
                    if (rootTask.Timeout > 0)
                    {
                        var cts = new CancellationTokenSource(rootTask.Timeout);
                        CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                        ct = cts.Token;
                    }

                    if (cancellationToken.IsCancellationRequested)
                    {
                        bus.QueueMessage(new TaskStartedMessage(rootTask));
                        bus.QueueMessage(new TaskFinishedMessage(rootTask, TaskStatus.Cancelled));
                        return TaskRunnerResult.Cancelled();
                    }

                    try
                    {
                        bus.QueueMessage(new TaskStartedMessage(rootTask));

                        await rootTask.RunAsync(ctx, ct)
                            .ConfigureAwait(false);

                        bus.QueueMessage(new TaskFinishedMessage(rootTask, TaskStatus.Completed));
                    }
                    catch (TaskCanceledException)
                    {
                        bus.QueueMessage(new TaskFinishedMessage(rootTask, TaskStatus.Cancelled));
                        return TaskRunnerResult.Cancelled();
                    }
                    catch (Exception ex)
                    {
                        bus.QueueMessage(new TaskErrorMessage(ex, rootTask));
                        bus.QueueMessage(new TaskFinishedMessage(rootTask, TaskStatus.Failed));
                        return TaskRunnerResult.Failed();
                    }
                }

                Collect(rootTask, tasks, set);
            }
            else
            {
                foreach (var target in options.Targets)
                {
                    var task = tasks[target];
                    if (task is null)
                    {
                        bus.QueueMessage(new ErrorMessage($"No tasks were found for {target}"));
                        return TaskRunnerResult.Failed();
                    }

                    if (options.SkipDependencies)
                    {
                        set.Add(task);
                        continue;
                    }

                    Collect(task, tasks, set);
                }
            }

            IExecutionContext? parentContext = context;
            bool failed = false;

            foreach (var task in tasks)
            {
                if (failed && !task.ContinueOnError)
                    continue;

                var ctx = parentContext != null
                    ? new TaskExecutionContext(task.Id, parentContext)
                    : new TaskExecutionContext(task.Id, this.services);

                var ct = cancellationToken;
                if (task.Timeout > 0)
                {
                    var cts = new CancellationTokenSource(task.Timeout);
                    CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                    ct = cts.Token;
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    bus.QueueMessage(new TaskStartedMessage(task));
                    bus.QueueMessage(new TaskFinishedMessage(task, TaskStatus.Cancelled));
                    return TaskRunnerResult.Cancelled();
                }

                try
                {
                    bus.QueueMessage(new TaskStartedMessage(task));

                    var result = await task.RunAsync(ctx, ct)
                        .ConfigureAwait(false);

                    if (result.Status == TaskStatus.Failed)
                    {
                        failed = true;
                    }

                    if (result.Status == TaskStatus.Cancelled)
                    {
                        bus.QueueMessage(new TaskFinishedMessage(task, TaskStatus.Cancelled));
                        return TaskRunnerResult.Cancelled();
                    }

                    bus.QueueMessage(new TaskFinishedMessage(task, result.Status));
                }
                catch (Exception ex)
                {
                    failed = true;
                    bus.QueueMessage(new TaskErrorMessage(ex, task));
                    bus.QueueMessage(new TaskFinishedMessage(task, TaskStatus.Failed));
                    continue;
                }

                parentContext = ctx;
            }

            if (failed)
            {
                return TaskRunnerResult.Failed();
            }

            return TaskRunnerResult.Success();
        }
        catch (Exception ex)
        {
            bus.QueueMessage(new ErrorMessage(ex));
            return TaskRunnerResult.Failed();
        }
    }

    private static void Collect(ITask task, IDependencyCollection<ITask> source, List<ITask> destination)
    {
        foreach (var dep in task.Dependencies)
        {
            var childTask = source[dep];
            if (childTask is null)
                throw new InvalidOperationException($"Task dependency {dep} was not found for task {task.Name}");

            Collect(childTask, source, destination);
        }

        if (!destination.Contains(task))
            destination.Add(task);
    }
}
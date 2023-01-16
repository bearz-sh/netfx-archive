using Ze.Tasks.Internal;

namespace Ze.Tasks;

public class WorkflowRunner
{
    private readonly IServiceProvider services;

    public WorkflowRunner(IServiceProvider services)
    {
        this.services = services;
    }

    public async Task<TaskStatus> RunTaskAsync(string target, TaskCollection tasks, CancellationToken cancellationToken = default)
    {
        var context = new WorkflowExecutionContext(this.services);
        var runner = new TaskRunner();
        var status = await runner.RunAsync(target, tasks, context, cancellationToken)
            .ConfigureAwait(false);

        return status;
    }

    public async Task<JobStatus> RunJobAsync(string target, JobCollection jobs, CancellationToken cancellationToken = default)
    {
        var context = new WorkflowExecutionContext(this.services);
        var runner = new JobRunner();
        var status = await runner.RunAsync(target, jobs, context, cancellationToken)
            .ConfigureAwait(false);

        return status;
    }
}
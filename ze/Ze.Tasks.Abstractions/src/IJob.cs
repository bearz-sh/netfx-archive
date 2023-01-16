namespace Ze.Tasks;

public interface IJob
{
    string Id { get; }

    string Name { get; }

    int Timeout { get; }

    IReadOnlyList<JobDependency> Dependencies { get; }

    IReadOnlyList<ITask> Tasks { get; }

    Task<JobStatus> RunAsync(IJobExecutionContext context, CancellationToken cancellationToken = default);
}
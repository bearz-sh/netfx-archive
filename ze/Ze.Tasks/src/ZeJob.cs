using Bearz.Text;

namespace Ze.Tasks;

public class ZeJob : IJob
{
    public ZeJob(string name)
       : this(name, name)
    {
    }

    public ZeJob(string name, string id)
    {
        this.Name = name;
        var sb = StringBuilderCache.Acquire();
        foreach (var c in id)
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

            sb.Append('_');
        }

        this.Id = StringBuilderCache.GetStringAndRelease(sb);
    }

    public string Id { get; }

    public string Name { get; }

    public int Timeout { get; set; }

    public IReadOnlyList<JobDependency> Dependencies { get; set; } = Array.Empty<JobDependency>();

    public List<ITask> Tasks { get; } = new();

    IReadOnlyList<ITask> IJob.Tasks => this.Tasks;

    public async Task<JobStatus> RunAsync(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        var runner = new TaskRunner();

        if (cancellationToken.IsCancellationRequested)
            return JobStatus.Cancelled;

        var ct = cancellationToken;

        if (this.Timeout > 0)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(this.Timeout);
            CancellationTokenSource.CreateLinkedTokenSource(cts.Token, cancellationToken);
            ct = cts.Token;
        }

        var status = await runner.RunAsync(this.Tasks, context, ct);
        switch (status)
        {
            case TaskStatus.Cancelled:
                return JobStatus.Cancelled;
            case TaskStatus.Failed:
                return JobStatus.Failed;

            case TaskStatus.Skipped:
                return JobStatus.Skipped;

            case TaskStatus.Completed:
                return JobStatus.Completed;

            default:
                return JobStatus.Completed;
        }
    }
}
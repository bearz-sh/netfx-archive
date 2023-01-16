namespace Ze.Tasks;

public class TaskResult
{
    public TaskStatus Status { get; private set; }

    public DateTimeOffset StartedAt { get; private set; }

    public DateTimeOffset FinishedAt { get; private set; }

    public void Skip()
    {
        this.StartedAt = DateTimeOffset.UtcNow;
        this.FinishedAt = this.StartedAt;
        this.Status = TaskStatus.Skipped;
    }

    public void Start()
    {
        this.StartedAt = DateTimeOffset.Now;
    }

    public void Error()
    {
        if (this.FinishedAt < this.StartedAt)
            this.FinishedAt = DateTimeOffset.UtcNow;

        this.Status = TaskStatus.Failed;
    }

    public void Complete()
    {
        if (this.FinishedAt < this.StartedAt)
            this.FinishedAt = DateTimeOffset.UtcNow;

        this.Status = TaskStatus.Completed;
    }

    public void Stop()
    {
        this.FinishedAt = DateTimeOffset.UtcNow;
    }
}
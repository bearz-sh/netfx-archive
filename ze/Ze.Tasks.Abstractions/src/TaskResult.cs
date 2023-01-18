namespace Ze.Tasks;

public class TaskResult
{
    internal TaskResult(TaskStatus status)
    {
        this.Status = status;
    }

    public TaskStatus Status { get; }

    public static TaskResult Failed()
    {
        return new TaskResult(TaskStatus.Failed);
    }

    public static TaskResult Cancelled()
    {
        return new TaskResult(TaskStatus.Failed);
    }

    public static TaskResult Completed()
    {
        return new TaskResult(TaskStatus.Completed);
    }
}
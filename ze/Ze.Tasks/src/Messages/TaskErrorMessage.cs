using Bearz.Std;

namespace Ze.Tasks.Messages;

public class TaskErrorMessage : ErrorMessage
{
    public TaskErrorMessage(string message, ITask task)
        : base(message)
    {
        this.Task = task;
    }

    public TaskErrorMessage(Exception exception, ITask task)
        : base(exception)
    {
        this.Task = task;
    }

    public TaskErrorMessage(Error error, ITask task)
        : base(error)
    {
        this.Task = task;
    }

    public ITask Task { get; }
}
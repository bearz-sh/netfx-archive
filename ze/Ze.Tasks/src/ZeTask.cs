using System;
using System.Linq;
using System.Runtime.InteropServices;

using Bearz.Text;

using Microsoft.Extensions.Primitives;

namespace Ze.Tasks;

public abstract class ZeTask : ITask
{
    protected ZeTask(string name)
        : this(name, name)
    {
    }

    protected ZeTask(string name, string id)
    {
        this.Name = name;
        this.Id = IdGenerator.Instance.FromName(id.AsSpan());
    }

    public string Id { get; }

    public string Name { get; set; }

    public virtual IReadOnlyList<string> Dependencies { get; set; } = Array.Empty<string>();

    public int Timeout { get; set; }

    public string? Description { get; set; }

    public bool ContinueOnError { get; set; }

    public abstract Task<TaskResult> RunAsync(
        ITaskExecutionContext context,
        CancellationToken cancellationToken = default);
}
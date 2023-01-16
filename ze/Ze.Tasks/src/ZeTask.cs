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

    public string Name { get; set; }

    public virtual IReadOnlyList<string> Dependencies { get; set; } = Array.Empty<string>();

    public int Timeout { get; set; }

    public string? Description { get; set; }

    public bool ContinueOnError { get; set; }

    public abstract Task<TaskStatus> RunAsync(
        ITaskExecutionContext context,
        CancellationToken cancellationToken = default);
}
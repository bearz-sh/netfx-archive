using System.Collections.ObjectModel;

namespace Ze.Tasks;

public class TaskCollection : KeyedCollection<string, ITask>
{
    public TaskCollection()
        : base(StringComparer.OrdinalIgnoreCase)
    {
    }

    public ZeTaskBuilder AddTask(string name, Action action)
    {
        var task = new ActionTask(name, action);
        this.Add(task);
        return new ZeTaskBuilder(task);
    }

    public ZeTaskBuilder AddTask(string name, string id, Action action)
    {
        var task = new ActionTask(name, id, action);
        this.Add(task);
        return new ZeTaskBuilder(task);
    }

    public ZeTaskBuilder AddTask(string name, Action<ITaskExecutionContext> action)
    {
        var task = new ActionTask(name, action);
        this.Add(task);
        return new ZeTaskBuilder(task);
    }

    public ZeTaskBuilder AddTask(string name, string id, Action<ITaskExecutionContext> action)
    {
        var task = new ActionTask(name, id, action);
        this.Add(task);
        return new ZeTaskBuilder(task);
    }

    public ZeTaskBuilder AddTask(string name, Func<ITaskExecutionContext, Task> action)
    {
        var task = new FuncTask(name, action);
        this.Add(task);
        return new ZeTaskBuilder(task);
    }

    public ZeTaskBuilder AddTask(string name, string id, Func<ITaskExecutionContext, Task> action)
    {
        var task = new FuncTask(name, id, action);
        this.Add(task);
        return new ZeTaskBuilder(task);
    }

    internal void CheckForMissingDependencies()
    {
        var missingDependencies = new SortedDictionary<string, SortedSet<string>>();

        foreach (var target in this)
        {
            foreach (var dependency in target.Dependencies.Where(dependency => !this.Contains(dependency)))
            {
                if (missingDependencies.TryGetValue(dependency, out var targets))
                {
                    targets.Add(target.Name);
                }
                else
                {
                    missingDependencies.Add(dependency, new SortedSet<string> { target.Name });
                }
            }
        }

        if (missingDependencies.Count == 0)
        {
            return;
        }

        var message =
            $"Missing {(missingDependencies.Count > 1 ? "dependencies" : "dependency")}: " +
            string.Join(
                "; ",
                missingDependencies.Select(dependency =>
                    $"{dependency.Key}, required by {dependency.Value}"));

        throw new InvalidOperationException(message);
    }

    internal void CheckForCircularDependencies()
    {
        var dependents = new Stack<string>();

        foreach (var task in this)
        {
            Check(task);
        }

        void Check(ITask target)
        {
            if (dependents.Contains(target.Name))
            {
                throw new InvalidOperationException($"Circular dependency: {string.Join(" -> ", dependents.Reverse().Append(target.Name))}");
            }

            dependents.Push(target.Name);

            foreach (var dependency in target.Dependencies.Where(this.Contains))
            {
                Check(this[dependency]);
            }

            _ = dependents.Pop();
        }
    }

    protected override string GetKeyForItem(ITask item)
    {
        return item.Id;
    }
}
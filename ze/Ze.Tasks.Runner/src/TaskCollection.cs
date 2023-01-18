using System.Collections.ObjectModel;

using Bearz.Extra.Strings;

namespace Ze.Tasks;

public class TaskCollection : Collection<ITask>, IDependencyCollection<ITask>
{
    public ITask? this[string nameOrId]
    {
        get
        {
            foreach (var task in this)
            {
                if (task.Id.EqualsIgnoreCase(nameOrId) || task.Name.EqualsIgnoreCase(nameOrId))
                    return task;
            }

            return null;
        }
    }

    public bool Contains(string name)
        => this[name] != null;

    public new void Add(ITask item)
    {
        foreach (var task in this)
        {
            if (task.Id.EqualsIgnoreCase(item.Id))
                throw new InvalidOperationException($"Task with id {task.Id} already exists in the collection");
        }

        base.Add(item);
    }

    public void CheckCircularDependencies()
    {
        throw new NotImplementedException();
    }

    public void CheckMissingDependencies()
    {
        throw new NotImplementedException();
    }
}
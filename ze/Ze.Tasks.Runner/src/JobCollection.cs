using System.Collections.ObjectModel;

using Bearz.Extra.Strings;

namespace Ze.Tasks;

public class JobCollection : Collection<IJob>, IDependencyCollection<IJob>
{
    public IJob? this[string name]
    {
        get
        {
            foreach (var job in this)
            {
                if (job.Id.EqualsIgnoreCase(name) || job.Name.EqualsIgnoreCase(job.Id))
                    return job;
            }

            return null;
        }
    }

    public new void Add(IJob item)
    {
        foreach (var job in this)
        {
            if (job.Id.EqualsIgnoreCase(item.Id))
                throw new InvalidOperationException($"Job with id {job.Id} already exists in the collection");
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
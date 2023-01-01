using System.Collections.ObjectModel;

namespace Casa.Data.Model;

public class Environment
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string LoweredName { get; set; } = string.Empty;

    public virtual HashSet<Secret> Secrets { get; set; } = new();

    public virtual HashSet<EnvironmentVariable> Variables { get; set; } = new();
}
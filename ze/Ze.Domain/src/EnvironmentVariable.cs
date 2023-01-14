using DbEnvironmentVariable = Ze.Data.Models.EnvironmentVariable;

namespace Ze.Domain;

public class EnvironmentVariable
{
    private readonly DbEnvironmentVariable model;

    public EnvironmentVariable()
    {
        this.model = new DbEnvironmentVariable();
    }

    internal EnvironmentVariable(DbEnvironmentVariable dbEnvironmentVariable)
    {
        this.model = dbEnvironmentVariable;
    }

    public string Name
    {
        get => this.model.Name;
        set => this.model.Name = value;
    }

    public string Value
    {
        get => this.model.Value;
        set => this.model.Value = value;
    }

    internal DbEnvironmentVariable Model => this.model;
}
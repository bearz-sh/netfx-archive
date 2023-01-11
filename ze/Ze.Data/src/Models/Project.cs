namespace Ze.Data.Models;

public class Project
{
    public static Project Default { get; } = new Project() { Id = -1, Name = "Default", Slug = "Default" };

    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public HashSet<ProjectRepository> Repositories { get; set; } = new HashSet<ProjectRepository>();

    public HashSet<Pipeline> Pipelines { get; set; } = new HashSet<Pipeline>();
}
using System.ComponentModel.DataAnnotations.Schema;

namespace Ze.Data.Models;

public class Pipeline
{
    public static Pipeline Default { get; } = new Pipeline { Id = 0, Name = "Default" };

    public int Id { get; set; }

    public int ProjectId { get; set; }

    [ForeignKey(nameof(ProjectId))]
    public Project Project { get; set; } = Ze.Data.Models.Project.Default;

    public int? ProjectRepositoryId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? JsonDefinition { get; set; } = string.Empty;

    public string? DefinitionFile { get; set; } = string.Empty;

    public HashSet<PipelineRun> Runs { get; set; } = new();
}
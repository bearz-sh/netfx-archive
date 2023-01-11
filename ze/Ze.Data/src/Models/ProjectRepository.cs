using System.ComponentModel.DataAnnotations.Schema;

namespace Ze.Data.Models;

public class ProjectRepository
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    [ForeignKey(nameof(ProjectId))]
    public Project? Project { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public string? Directory { get; set; }

    public bool Default { get; set; }
}
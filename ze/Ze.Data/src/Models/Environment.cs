using System.ComponentModel.DataAnnotations.Schema;

namespace Ze.Data.Models;

public class Environment
{
    public int Id { get; set; }

    public int? PipelineId { get; set; }

    [ForeignKey(nameof(PipelineId))]
    public Pipeline? Pipeline { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public virtual HashSet<EnvironmentSecret> Secrets { get; set; } = new();

    public virtual HashSet<EnvironmentVariable> Variables { get; set; } = new();
}
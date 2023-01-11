using System.ComponentModel.DataAnnotations.Schema;

namespace Ze.Data.Models;

public class PipelineRun
{
    public int Id { get; set; }

    public int PipelineId { get; set; }

    [ForeignKey(nameof(PipelineId))]
    public Pipeline Pipeline { get; set; } = Pipeline.Default;

    public int? ProjectRepositoryId { get; set; }

    [ForeignKey(nameof(ProjectRepositoryId))]
    public ProjectRepository? ProjectRepository { get; set; }

    public int Revision { get; set; }

    public int Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public string? BranchName { get; set; }

    public string? CommitRef { get; set; }

    public string? CommitEmail { get; set; }

    public string? StateJson { get; set; }
}
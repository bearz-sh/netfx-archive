using System.ComponentModel.DataAnnotations.Schema;

namespace Ze.Data.Models;

public class EnvironmentVariable
{
    public int Id { get; set; }

    public int EnvironmentId { get; set; }

    [ForeignKey(nameof(EnvironmentId))]
    public virtual Environment? Environment { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;
}
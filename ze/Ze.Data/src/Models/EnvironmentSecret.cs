using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Ze.Data.Models;

public class EnvironmentSecret
{
    private Dictionary<string, string>? tagsCache;

    public int Id { get; set; }

    public int EnvironmentId { get; set; }

    [ForeignKey(nameof(EnvironmentId))]
    public virtual Environment? Environment { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    public DateTime? ExpiresAt { get; set; }

    public string JsonTags { get; set; } = string.Empty;

    [NotMapped]
    public IDictionary<string, string> Tags
    {
        get
        {
            if (this.tagsCache is not null)
                return this.tagsCache;

            var data = JsonSerializer.Deserialize<Dictionary<string, string>>(this.JsonTags) ?? new Dictionary<string, string>();
            this.tagsCache = new Dictionary<string, string>(data, StringComparer.OrdinalIgnoreCase);
            return this.tagsCache;
        }
        set => this.JsonTags = JsonSerializer.Serialize(value);
    }
}
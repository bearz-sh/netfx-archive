namespace Bearz.Extensions.Secrets;

public interface ISecretRecord
{
    string Name { get; }

    string Value { get; set; }

    DateTime? ExpiresAt { get; set; }

    DateTime? CreatedAt { get; set; }

    DateTime? UpdatedAt { get; set; }

    IDictionary<string, object?> Properties { get; }

    IDictionary<string, string?> Tags { get; }
}
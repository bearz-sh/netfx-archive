using System.Collections.Concurrent;

namespace Bearz.Extensions.Secrets;

public class MemorySecretVault : ISecretsVault, ISecretVaultMetaData
{
    private readonly ConcurrentDictionary<string, ISecretRecord> secrets = new(StringComparer.OrdinalIgnoreCase);

    public Task<IEnumerable<string>> ListNamesAsync()
        => Task.FromResult(this.ListNames());

    public IEnumerable<string> ListNames()
        => this.secrets.Keys;

    public Task<string?> GetSecretAsync(string name)
        => Task.FromResult<string?>(this.GetSecret(name));

    public string? GetSecret(string name)
    {
         if (this.secrets.TryGetValue(name, out var secret))
         {
             return secret.Value;
         }

         return null;
    }

    public Task SetSecretAsync(string name, string secret)
    {
        this.SetSecret(name, secret);
        return Task.CompletedTask;
    }

    public void SetSecret(string name, string secret)
    {
        if (this.secrets.TryGetValue(name, out var record))
        {
            record.Value = secret;
            this.SetSecretRecord(record);
            return;
        }

        record = this.CreateRecord(name);
        record.Value = secret;
        this.SetSecretRecord(record);
    }

    public Task DeleteSecretAsync(string name)
    {
        this.DeleteSecret(name);
        return Task.CompletedTask;
    }

    public void DeleteSecret(string name)
    {
        this.secrets.TryRemove(name, out _);
    }

    public ISecretRecord CreateRecord(string name)
        => new MemorySecretRecord(name);

    public Task<ISecretRecord?> GetSecretRecordAsync(string name)
        => Task.FromResult(this.GetSecretRecord(name));

    public ISecretRecord? GetSecretRecord(string name)
    {
        if (this.secrets.TryGetValue(name, out var record))
            return record;

        return null;
    }

    public void SetSecretRecord(ISecretRecord record)
    {
        var existing = this.GetSecretRecord(record.Name);
        if (existing is null)
        {
            record.CreatedAt = DateTime.UtcNow;
            this.secrets[record.Name] = record;
            record.Properties["__version"] = 1;
            return;
        }

        existing.Value = record.Value;
        existing.UpdatedAt = DateTime.UtcNow;
        existing.ExpiresAt = record.ExpiresAt;
        var v = 1;
        if (existing.Properties.TryGetValue("__version", out object? version) && version != null)
        {
            v = (int)version + 1;
        }

        existing.Properties.Clear();

        foreach (var prop in record.Properties)
        {
            if (prop.Key == "__version")
                continue;

            existing.Properties[prop.Key] = prop.Value;
        }

        existing.Properties["__version"] = v;
        existing.Tags.Clear();
        foreach (var tag in record.Tags)
        {
            existing.Tags.Add(tag);
        }
    }

    public Task SetSecretRecordAsync(ISecretRecord record)
    {
        this.SetSecretRecord(record);
        return Task.CompletedTask;
    }

    internal sealed class MemorySecretRecord : ISecretRecord
    {
        public MemorySecretRecord(string name)
        {
            this.Name = name;
        }

        public string Name { get; }

        public string Value { get; set; } = string.Empty;

        public DateTime? ExpiresAt { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public IDictionary<string, object?> Properties { get; } = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);

        public IDictionary<string, string?> Tags { get; } = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
    }
}
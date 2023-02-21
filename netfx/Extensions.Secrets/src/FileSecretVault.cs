using System.Collections.Concurrent;
using System.Text.Json;

using Bearz.Extra.Strings;
using Bearz.Security.Cryptography;
using Bearz.Text;

namespace Bearz.Extensions.Secrets;

public class FileSecretVault : ISecretsVault, ISecretVaultMetaData
{
    private readonly IEncryptionProvider cipher;

    private readonly string filePath;

    private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

    private ConcurrentDictionary<string, MemorySecretRecord>? data;

    public FileSecretVault(string filePath, IEncryptionProvider cipher)
    {
        this.filePath = Path.GetFullPath(filePath);
        this.cipher = cipher;
    }

    public async Task<IEnumerable<string>> ListNamesAsync(CancellationToken cancellationToken = default)
    {
        var data = await this.GetOrLoadDataAsync(cancellationToken)
            .ConfigureAwait(false);

        var keys = data.Keys.ToArray();
        return keys;
    }

    public IEnumerable<string> ListNames()
    {
        var data = this.GetOrLoadData();
        return data.Keys.ToArray();
    }

    public async Task<string?> GetSecretAsync(string name, CancellationToken cancellationToken = default)
    {
        name = NormalizeSecretKey(name);
        var data = await this.GetOrLoadDataAsync(cancellationToken)
            .ConfigureAwait(false);
        if (!data.TryGetValue(name, out var record) || record is null)
        {
            return default;
        }

        var encryptedBytes = Convert.FromBase64String(record.Value);
        var decryptedBytes = this.cipher.Decrypt(encryptedBytes);
        return Encodings.Utf8NoBom.GetString(decryptedBytes);
    }

    public string? GetSecret(string name)
    {
        name = NormalizeSecretKey(name);
        var data = this.GetOrLoadData();
        if (!data.TryGetValue(name, out var record) || record is null)
        {
            return default;
        }

        var encryptedBytes = Convert.FromBase64String(record.Value);
        var decryptedBytes = this.cipher.Decrypt(encryptedBytes);
        return Encodings.Utf8NoBom.GetString(decryptedBytes);
    }

#pragma warning disable S4457
    public async Task SetSecretAsync(string name, string secret, CancellationToken cancellationToken = default)
#pragma warning restore S4457
    {
        if (name.IsNullOrWhiteSpace())
            throw new ArgumentNullException(nameof(name));

        if (secret.IsNullOrWhiteSpace())
            throw new ArgumentNullException(nameof(secret));

        name = NormalizeSecretKey(name);

        var data = await this.GetOrLoadDataAsync(cancellationToken)
            .ConfigureAwait(false);

        var bytes = Encodings.Utf8NoBom.GetBytes(secret);
        var encrypted = this.cipher.Encrypt(bytes);
        var value = Convert.ToBase64String(encrypted);

        if (!data.TryGetValue(name, out var record) || record is null)
        {
            record = new MemorySecretRecord()
            {
                Name = name,
                Value = value,
                CreatedAt = DateTime.UtcNow,
            };
        }
        else
        {
            record.Value = value;
            record.UpdatedAt = DateTime.UtcNow;
        }

        data[name] = record;
        await this.SaveDataAsync(data, cancellationToken)
            .ConfigureAwait(false);
    }

    public void SetSecret(string name, string secret)
    {
        if (name.IsNullOrWhiteSpace())
            throw new ArgumentNullException(nameof(name));

        if (secret.IsNullOrWhiteSpace())
            throw new ArgumentNullException(nameof(secret));

        name = NormalizeSecretKey(name);
        var data = this.GetOrLoadData();
        var bytes = Encodings.Utf8NoBom.GetBytes(secret);
        var encrypted = this.cipher.Encrypt(bytes);
        var value = Convert.ToBase64String(encrypted);

        if (!data.TryGetValue(name, out var record) || record is null)
        {
            record = new MemorySecretRecord()
            {
                Name = name,
                Value = value,
                CreatedAt = DateTime.UtcNow,
            };
        }
        else
        {
            record.Value = value;
            record.UpdatedAt = DateTime.UtcNow;
        }

        data[name] = record;
        this.SaveData(data);
    }

    public async Task DeleteSecretAsync(string name, CancellationToken cancellationToken = default)
    {
        name = NormalizeSecretKey(name);
        var data = await this.GetOrLoadDataAsync(cancellationToken)
            .ConfigureAwait(false);

        if (!data.ContainsKey(name))
            return;

        data.Remove(name);
    }

    public void DeleteSecret(string name)
    {
        name = NormalizeSecretKey(name);
        var data = this.GetOrLoadData();
        if (!data.ContainsKey(name))
            return;

        data.Remove(name);
        this.SaveData(data);
    }

    public ISecretRecord CreateRecord(string name)
    {
        name = NormalizeSecretKey(name);
        return new MemorySecretRecord() { Name = name };
    }

    public async Task<ISecretRecord?> GetSecretRecordAsync(string name,  CancellationToken cancellationToken = default)
    {
        name = NormalizeSecretKey(name);
        var data = await this.GetOrLoadDataAsync(cancellationToken)
            .ConfigureAwait(false);

        if (!data.ContainsKey(name))
            return null;

        return data[name];
    }

    public ISecretRecord? GetSecretRecord(string name)
    {
        name = NormalizeSecretKey(name);
        var data = this.GetOrLoadData();
        if (!data.TryGetValue(name, out var record) || record is null)
            return null;

        var encryptedBytes = Convert.FromBase64String(record.Value);
        var decryptedBytes = this.cipher.Decrypt(encryptedBytes);
        var decryptedValue = Encodings.Utf8NoBom.GetString(decryptedBytes);

        return new MemorySecretRecord()
        {
            Name = record.Name,
            Value = decryptedValue,
            CreatedAt = record.CreatedAt,
            UpdatedAt = record.UpdatedAt,
            Properties = record.Properties,
            Tags = record.Tags,
            ExpiresAt = record.ExpiresAt,
        };
    }

    public void SetSecretRecord(ISecretRecord record)
    {
        if (record is null)
            throw new ArgumentNullException(nameof(record));

        if (record.Name.IsNullOrWhiteSpace())
            throw new ArgumentException("Secret name cannot be null or empty", nameof(record));

        if (record.Value.IsNullOrWhiteSpace())
            throw new ArgumentException("Secret value cannot be null or empty", nameof(record));

        var data = this.GetOrLoadData();
        var bytes = Encodings.Utf8NoBom.GetBytes(record.Value);
        var encrypted = this.cipher.Encrypt(bytes);
        var secret = Convert.ToBase64String(encrypted);

        var name = NormalizeSecretKey(record.Name);
        data[name] = new MemorySecretRecord()
        {
            Name = name,
            Value = secret,
            ExpiresAt = record.ExpiresAt,
            CreatedAt = record.CreatedAt,
            UpdatedAt = record.UpdatedAt,
            Properties = record.Properties,
            Tags = record.Tags,
        };

        this.SaveData(data);
    }

#pragma warning disable S4457
    public async Task SetSecretRecordAsync(ISecretRecord record, CancellationToken cancellationToken = default)
#pragma warning restore S4457
    {
        if (record is null)
            throw new ArgumentNullException(nameof(record));

        if (record.Name.IsNullOrWhiteSpace())
            throw new ArgumentException("Secret name cannot be null or empty", nameof(record));

        if (record.Value.IsNullOrWhiteSpace())
            throw new ArgumentException("Secret value cannot be null or empty", nameof(record));

        var name = NormalizeSecretKey(record.Name);

        var data = await this.GetOrLoadDataAsync(cancellationToken)
            .ConfigureAwait(false);
        var bytes = Encodings.Utf8NoBom.GetBytes(record.Value);
        var encrypted = this.cipher.Encrypt(bytes);
        var secret = Convert.ToBase64String(encrypted);

        data[name] = new MemorySecretRecord()
        {
            Name = name,
            Value = secret,
            ExpiresAt = record.ExpiresAt,
            CreatedAt = record.CreatedAt,
            UpdatedAt = record.UpdatedAt,
            Properties = record.Properties,
            Tags = record.Tags,
        };

        await this.SaveDataAsync(data, cancellationToken)
            .ConfigureAwait(false);
    }

    private static string NormalizeSecretKey(string key)
    {
        var sb = Bearz.Text.StringBuilderCache.Acquire();
        foreach (var c in key)
        {
            if (char.IsLetterOrDigit(c))
            {
                sb.Append(c);
            }
            else if (c is '-' or '.' or '_' or '/' or ':' or ' ')
            {
                sb.Append('-');
            }
            else
            {
                throw new InvalidOperationException($"Invalid character '{c}' in secret key '{key}'");
            }
        }

        return Text.StringBuilderCache.GetStringAndRelease(sb);
    }

    private void SaveData(IDictionary<string, MemorySecretRecord> data)
    {
        this.semaphore.Wait();
        try
        {
            var dir = Path.GetDirectoryName(this.filePath)!;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var lockFile = $"{this.filePath}.lock";
            int count = 0;
            do
            {
                if (count > 100)
                    throw new TimeoutException("Could not acquire lock on file");

                Thread.Sleep(1000);

                count++;
            }
            while (File.Exists(lockFile));

            try
            {
                File.WriteAllText(lockFile, string.Empty);
                var json = JsonSerializer.Serialize(data.Values.ToArray());
                File.WriteAllText(this.filePath, json);
            }
            finally
            {
                File.Delete(lockFile);
            }
        }
        finally
        {
            this.semaphore.Release();
        }
    }

    private async Task SaveDataAsync(
        IDictionary<string, MemorySecretRecord> data,
        CancellationToken cancellationToken = default)
    {
        await this.semaphore.WaitAsync(cancellationToken)
            .ConfigureAwait(false);

        try
        {
            var dir = Path.GetDirectoryName(this.filePath)!;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var lockFile = $"{this.filePath}.lock";
            int count = 0;
            do
            {
                if (count > 100)
                    throw new TimeoutException("Could not acquire lock on file");

                await Task.Delay(1000, cancellationToken)
                    .ConfigureAwait(false);

                count++;
            }
            while (File.Exists(lockFile));

            try
            {
#if NETLEGACY
                File.WriteAllText(lockFile, string.Empty);
#else
                await File.WriteAllTextAsync(lockFile, string.Empty);
#endif
                using var fs = File.Create(this.filePath);
                await JsonSerializer.SerializeAsync(
                    fs,
                    data.Values.ToArray(),
                    new JsonSerializerOptions()
                    {
                        WriteIndented = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    },
                    cancellationToken)
                    .ConfigureAwait(false);
            }
            finally
            {
                File.Delete(lockFile);
            }
        }
        finally
        {
            this.semaphore.Release();
        }
    }

    private IDictionary<string, MemorySecretRecord> GetOrLoadData()
    {
        if (this.data is not null)
            return this.data;

        this.semaphore.Wait();

        try
        {
            if (this.data is not null)
                return this.data;

            var data = new ConcurrentDictionary<string, MemorySecretRecord>(StringComparer.OrdinalIgnoreCase);
            if (File.Exists(this.filePath))
            {
                var json = File.ReadAllText(this.filePath);
                var records = JsonSerializer.Deserialize<MemorySecretRecord[]>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (records is not null)
                {
                    foreach (var record in records)
                    {
                        data.TryAdd(record.Name, record);
                    }
                }
            }

            this.data = data;
            return this.data;
        }
        finally
        {
            this.semaphore.Release();
        }
    }

    private async Task<IDictionary<string, MemorySecretRecord>> GetOrLoadDataAsync(CancellationToken cancellationToken = default)
    {
        if (this.data is not null)
            return this.data;

        await this.semaphore.WaitAsync(cancellationToken)
            .ConfigureAwait(false);

        try
        {
            var data = new ConcurrentDictionary<string, MemorySecretRecord>(StringComparer.OrdinalIgnoreCase);
            if (File.Exists(this.filePath))
            {
                var fs = File.OpenRead(this.filePath);
                var records = await JsonSerializer
                    .DeserializeAsync<MemorySecretRecord[]>(
                        fs,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true },
                        cancellationToken)
                    .ConfigureAwait(false);

                if (records is not null)
                {
                    foreach (var record in records)
                    {
                        data.TryAdd(record.Name, record);
                    }
                }
            }

            this.data = data;
            return this.data;
        }
        finally
        {
            this.semaphore.Release();
        }
    }

    internal sealed class MemorySecretRecord : ISecretRecord
    {
        public MemorySecretRecord()
        {
        }

        public string Name { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;

        public DateTime? ExpiresAt { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public IDictionary<string, object?> Properties { get; set; } =
            new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);

        public IDictionary<string, string?> Tags { get; set; } =
            new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
    }
}
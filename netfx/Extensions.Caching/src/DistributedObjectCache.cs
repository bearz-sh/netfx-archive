using System.Text.Json;

using Bearz.Extensions.Caching.Abstractions;

using Microsoft.Extensions.Caching.Distributed;

namespace Bearz.Extensions.Caching;

public class DistributedObjectCache : IObjectCache
{
    private readonly IDistributedCache cache;

    public DistributedObjectCache(IDistributedCache cache)
    {
        this.cache = cache;
    }

    protected internal JsonSerializerOptions JsonOptions { get; } = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        WriteIndented = false,
    };

    public virtual T? Get<T>(string key)
    {
        var bytes = this.cache.Get(key);
        if (bytes is null)
            return default;

        return JsonSerializer.Deserialize<T>(bytes, this.JsonOptions);
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var bytes = await this.cache.GetAsync(key, cancellationToken)
            .ConfigureAwait(false);

        if (bytes is null)
            return default;

        using var stream = new MemoryStream(bytes);
        return await JsonSerializer.DeserializeAsync<T>(stream, this.JsonOptions, cancellationToken)
            .ConfigureAwait(false);
    }

    public void Set<T>(string key, T value, ICacheExpirationOptions options)
    {
        var distributedOptions = new DistributedObjectCacheEntryOptions(options);
        var bytes = JsonSerializer.SerializeToUtf8Bytes(value, this.JsonOptions);
        this.cache.Set(key, bytes, distributedOptions);
    }

    public Task SetAsync<T>(string key, T value, ICacheExpirationOptions options, CancellationToken cancellationToken = default)
    {
        var distributedOptions = new DistributedObjectCacheEntryOptions(options);
        var bytes = JsonSerializer.SerializeToUtf8Bytes(value, this.JsonOptions);
        return this.cache.SetAsync(key, bytes, distributedOptions, cancellationToken);
    }

    public void Remove(string key)
    {
        this.cache.Remove(key);
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        => this.cache.RemoveAsync(key, cancellationToken);

    public void Refresh(string key)
        => this.cache.Refresh(key);

    public Task RefreshAsync(string key, CancellationToken cancellationToken = default)
        => this.cache.RefreshAsync(key, cancellationToken);
}
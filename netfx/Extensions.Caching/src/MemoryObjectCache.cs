using System;
using System.Collections.Concurrent;
using System.Linq;

using Bearz.Extensions.Caching.Abstractions;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Bearz.Extensions.Caching;

public class MemoryObjectCache : IObjectCache
{
    private readonly IMemoryCache cache;

    public MemoryObjectCache(IMemoryCache cache)
    {
        this.cache = cache;
    }

    public virtual T? Get<T>(string key)
        => this.cache.Get<T>(key);

    public virtual Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        => Task.FromResult(this.Get<T>(key));

    public virtual void Set<T>(string key, T value, ICacheExpirationOptions options)
    {
        this.cache.Set(key, value, new MemoryObjectCacheEntryOptions(options));
    }

    public virtual Task SetAsync<T>(
        string key,
        T value,
        ICacheExpirationOptions options,
        CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            cancellationToken.ThrowIfCancellationRequested();

        this.cache.Set(key, value, new MemoryObjectCacheEntryOptions(options));
        return Task.CompletedTask;
    }

    public void Remove(string key)
    {
        this.cache.Remove(key);
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            cancellationToken.ThrowIfCancellationRequested();

        this.cache.Remove(key);
        return Task.CompletedTask;
    }

    public void Refresh(string key)
    {
        throw new NotImplementedException(nameof(this.Refresh));
    }

    public Task RefreshAsync(string key, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException(nameof(this.RefreshAsync));
    }
}
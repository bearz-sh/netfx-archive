namespace Bearz.Extensions.Caching.Abstractions;

public interface IObjectCache
{
    T? Get<T>(string key);

    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);

    void Set<T>(string key, T value, ICacheExpirationOptions options);

    Task SetAsync<T>(string key, T value, ICacheExpirationOptions options, CancellationToken cancellationToken = default);

    void Remove(string key);

    Task RemoveAsync(string key, CancellationToken cancellationToken = default);

    void Refresh(string key);

    Task RefreshAsync(string key, CancellationToken cancellationToken = default);
}
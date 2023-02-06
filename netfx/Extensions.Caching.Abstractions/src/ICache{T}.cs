namespace Bearz.Extensions.Caching.Abstractions;

public interface ICache<T>
{
    string Key { get; }

    T? Get();

    Task<T?> GetAsync(CancellationToken cancellationToken = default);

    void Set(T value);

    Task SetAsync(T value, CancellationToken cancellationToken = default);

    void Remove();

    Task RemoveAsync(CancellationToken cancellationToken = default);
}
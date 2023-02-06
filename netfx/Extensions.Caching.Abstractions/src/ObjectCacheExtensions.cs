namespace Bearz.Extensions.Caching.Abstractions;

public static class ObjectCacheExtensions
{
    public static bool TryGetValue<T>(this IObjectCache cache, string key, out T? value)
    {
        var result = cache.Get<T>(key);
        if (result is null)
        {
            value = default;
            return false;
        }

        value = result;
        return true;
    }

    public static void Set<T>(this IObjectCache cache, string key, T value)
    {
        cache.Set<T>(key, value, new CacheExpirationOptions());
    }

    public static Task SetAsync<T>(this IObjectCache cache, string key, T value)
    {
        return cache.SetAsync<T>(key, value, new CacheExpirationOptions());
    }
}
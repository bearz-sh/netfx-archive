using Bearz.Extensions.Caching.Abstractions;

using Microsoft.Extensions.Caching.Memory;

namespace Bearz.Extensions.Caching;

public class MemoryObjectCacheEntryOptions : MemoryCacheEntryOptions, ICacheExpirationOptions
{
    public MemoryObjectCacheEntryOptions()
    {
    }

    public MemoryObjectCacheEntryOptions(ICacheExpirationOptions options)
    {
        if (options is MemoryObjectCacheEntryOptions mo)
        {
            if (mo.AbsoluteExpiration.HasValue)
                options.AbsoluteExpiration = mo.AbsoluteExpiration;

            if (mo.SlidingExpiration.HasValue)
                options.SlidingExpiration = mo.SlidingExpiration;

            if (mo.AbsoluteExpirationRelativeToNow.HasValue)
                options.AbsoluteExpirationRelativeToNow = mo.AbsoluteExpirationRelativeToNow;

            this.Priority = mo.Priority;
            this.Size = mo.Size;

            foreach (var next in mo.PostEvictionCallbacks)
                this.PostEvictionCallbacks.Add(next);

            return;
        }

        if (options.AbsoluteExpiration.HasValue)
            this.AbsoluteExpiration = options.AbsoluteExpiration;

        if (options.SlidingExpiration.HasValue)
            this.SlidingExpiration = options.SlidingExpiration;

        if (options.AbsoluteExpirationRelativeToNow.HasValue)
            this.AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow;
    }
}
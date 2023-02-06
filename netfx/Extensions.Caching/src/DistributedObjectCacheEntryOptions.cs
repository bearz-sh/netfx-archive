using Bearz.Extensions.Caching.Abstractions;

using Microsoft.Extensions.Caching.Distributed;

namespace Bearz.Extensions.Caching;

public class DistributedObjectCacheEntryOptions : DistributedCacheEntryOptions, ICacheExpirationOptions
{
    public DistributedObjectCacheEntryOptions()
    {
    }

    public DistributedObjectCacheEntryOptions(ICacheExpirationOptions options)
    {

        if (options.AbsoluteExpiration.HasValue)
            this.AbsoluteExpiration = options.AbsoluteExpiration;

        if (options.SlidingExpiration.HasValue)
            this.SlidingExpiration = options.SlidingExpiration;

        if (options.AbsoluteExpirationRelativeToNow.HasValue)
            this.AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow;
    }
}
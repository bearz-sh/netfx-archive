namespace Bearz.Extensions.Caching.Abstractions;

public class CacheExpirationOptions : ICacheExpirationOptions
{
    public DateTimeOffset? AbsoluteExpiration { get; set; }

    public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }

    public TimeSpan? SlidingExpiration { get; set; }
}
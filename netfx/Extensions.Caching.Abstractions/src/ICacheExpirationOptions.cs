namespace Bearz.Extensions.Caching.Abstractions;

public interface ICacheExpirationOptions
{
    DateTimeOffset? AbsoluteExpiration { get; set; }

    TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }

    TimeSpan? SlidingExpiration { get; set; }
}
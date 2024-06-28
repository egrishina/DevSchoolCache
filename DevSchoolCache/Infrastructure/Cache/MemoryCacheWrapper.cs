using Microsoft.Extensions.Caching.Memory;

public interface ICacheWrapper
{
    bool TryGetValue<TItem>(object key, out TItem value);
    void Set<TItem>(object key, TItem value, MemoryCacheEntryOptions options);
}

public class MemoryCacheWrapper : ICacheWrapper
{
    private readonly IMemoryCache _memoryCache;

    public MemoryCacheWrapper(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public bool TryGetValue<TItem>(object key, out TItem value)
    {
        return _memoryCache.TryGetValue(key, out value);
    }

    public void Set<TItem>(object key, TItem value, MemoryCacheEntryOptions options)
    {
        _memoryCache.Set(key, value, options);
    }
}
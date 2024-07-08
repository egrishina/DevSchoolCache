using Microsoft.Extensions.Caching.Memory;

namespace DevSchoolCache;

public interface IHybridCacheService<TEntity>
{
    Task<TEntity?> GetOrAddAsync(string key,  Func<TEntity?> valueFactory);
}

public class HybridCacheService<TEntity>(
    ICacheWrapper inMemoryCache,
    IRedisAdapter redis) : IHybridCacheService<TEntity>
    where TEntity : class
{
    public async Task<TEntity?> GetOrAddAsync(string key, Func<TEntity?> valueFactory)
    {
        if (inMemoryCache.TryGetValue(key, out TEntity? entity))
            return entity;

        var cachedEntity = await redis.TryGetValueAsync<TEntity?>(key);
        if (cachedEntity.Exists)
            return cachedEntity.Entity;

        entity = valueFactory();

        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(60));

        inMemoryCache.Set(key, entity, cacheEntryOptions);
        await redis.TryAddValueAsync(key, entity, TimeSpan.FromMinutes(5));

        return entity;
    }
}
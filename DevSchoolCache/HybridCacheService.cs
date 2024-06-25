using Microsoft.Extensions.Caching.Memory;

namespace DevSchoolCache;

public class HybridCacheService<TEntity>(
    IMemoryCache inMemoryCache,
    IRedisAdapter redis,
    IRepository<TEntity> repository) : IHybridCacheService<TEntity>
    where TEntity : class
{
    public async Task<TEntity?> GetOrAddAsync(long id)
    {
        var key = FromIdToKey(id);
        
        if (inMemoryCache.TryGetValue(key, out TEntity? entity))
            return entity;

        entity = await redis.TryGetValueAsync<TEntity?>(key);
        if (entity is not null)
            return entity;

        entity = repository.TryGetById(FromKeyToId(key));

        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(60));

        inMemoryCache.Set(key, entity, cacheEntryOptions);
        await redis.TryAddValueAsync(key, entity, TimeSpan.FromMinutes(5));
        
        return entity;
    }

    private string FromIdToKey(long id)
    {
        return $"{nameof(TEntity)}.{id}";
    }

    private long FromKeyToId(string key)
    {
        return 1;
    }
}

public interface IHybridCacheService<TEntity>
{
    Task<TEntity?> GetOrAddAsync(long id);
}
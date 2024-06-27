using Microsoft.Extensions.Caching.Memory;

namespace DevSchoolCache;

public interface IHybridCacheService<TEntity>
{
    Task<TEntity?> GetOrAddAsync(long id);
}

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

        entity = repository.TryGetById(id);

        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(60));

        inMemoryCache.Set(key, entity, cacheEntryOptions);
        await redis.TryAddValueAsync(key, entity, TimeSpan.FromMinutes(5));
        
        return entity;
    }

    private string FromIdToKey(long id)
    {
        return $"{typeof(TEntity).Name}.{id}";
    }
}
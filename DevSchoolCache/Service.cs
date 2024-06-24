using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace DevSchoolCache;

public class Service<TEntity> where TEntity : class
{
    private readonly IMemoryCache _inMemoryCache;
    private readonly IRedisAdapter _redis;
    private readonly RepositoryBase<DbContext, TEntity> _repository;

    public Service(IMemoryCache inMemoryCache, IRedisAdapter redis, RepositoryBase<DbContext, TEntity> repository)
    {
        _inMemoryCache = inMemoryCache;
        _redis = redis;
        _repository = repository;
    }

    public async Task<TEntity?> GetOrAddAsync(string key)
    {
        if (_inMemoryCache.TryGetValue(key, out TEntity? entity))
            return entity;

        if (await _redis.TryGetValueAsync(key, out entity))
            return entity;

        entity = _repository.TryGetById(FromKeyToId(key));

        _inMemoryCache.CreateEntry(key);
        await _redis.TryAddValueAsync(key, entity);
        
        return entity;
    }

    private long FromKeyToId(string key)
    {
        return 1;
    }
}
namespace DevSchoolCache;

public class Service
{
    private readonly IMemoryCacheAdapter<Item> _inMemoryCache;
    private readonly IRedisAdapter<Item> _redis;
    private readonly IRepository<Item> _repository;

    public Service(IMemoryCacheAdapter<Item> inMemoryCache, IRedisAdapter<Item> redis, IRepository<Item> repository)
    {
        _inMemoryCache = inMemoryCache;
        _redis = redis;
        _repository = repository;
    }

    public async Task<Item> GetItem(string key)
    {
        return await _inMemoryCache.GetOrAddAsync(key);
    }
}
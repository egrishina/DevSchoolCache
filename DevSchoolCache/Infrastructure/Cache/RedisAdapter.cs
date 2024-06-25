using Newtonsoft.Json;
using StackExchange.Redis;

namespace DevSchoolCache;

public class RedisAdapter : IRedisAdapter
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisAdapter(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async Task<bool> TryAddValueAsync<TValue>(string key, TValue value, TimeSpan? expiry = null)
    {
        var database = _connectionMultiplexer.GetDatabase();
        var serializedValue = JsonConvert.SerializeObject(value);

        return await database.StringSetAsync(key, serializedValue, expiry);
    }

    public async Task DeleteValueAsync(string key)
    {
        var database = _connectionMultiplexer.GetDatabase();
        await database.KeyDeleteAsync(key);
    }

    public async Task<TValue?> TryGetValueAsync<TValue>(string key)
    {
        var database = _connectionMultiplexer.GetDatabase();
        var redisValue = await database.StringGetAsync(key);

        return redisValue.IsNullOrEmpty ? default : JsonConvert.DeserializeObject<TValue>(redisValue!);
    }
}  
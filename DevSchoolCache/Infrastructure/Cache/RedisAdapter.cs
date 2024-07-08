using System.Text.Json;
using StackExchange.Redis;

namespace DevSchoolCache;

public class RedisAdapter : IRedisAdapter
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisAdapter(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async Task<bool> TryAddValueAsync<TValue>(string key, TValue? value, TimeSpan? expiry = null)
    {
        var database = _connectionMultiplexer.GetDatabase();
        var serializedValue = JsonSerializer.Serialize(value);

        return await database.StringSetAsync(key, serializedValue, expiry);
    }

    public async Task DeleteValueAsync(string key)
    {
        var database = _connectionMultiplexer.GetDatabase();
        await database.KeyDeleteAsync(key);
    }

    public async Task<CachedEntity<TValue?>> TryGetValueAsync<TValue>(string key)
    {
        var database = _connectionMultiplexer.GetDatabase();
        var redisValue = await database.StringGetAsync(key);

        if (redisValue == RedisValue.Null) // если по ключу ничего нет
            return new CachedEntity<TValue?>(default, false);

        try
        {
            var deserializeObject = JsonSerializer.Deserialize<TValue>(redisValue!);

            if (deserializeObject is null) // если закеширован NUll
                return new CachedEntity<TValue?>(default, true);
            
            return new CachedEntity<TValue?>(deserializeObject, true);
        }
        catch (JsonException e)
        {
            // сломан контракт
            return new CachedEntity<TValue?>(default, false);
        }
    }
}  
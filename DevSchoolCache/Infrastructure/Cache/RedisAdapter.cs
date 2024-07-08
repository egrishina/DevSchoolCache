using System.Text.Json;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace DevSchoolCache;

public class RedisAdapter : IRedisAdapter
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly Logger<RedisAdapter> _logger;

    public RedisAdapter(IConnectionMultiplexer connectionMultiplexer, Logger<RedisAdapter> logger)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _logger = logger;
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

        if (redisValue == RedisValue.Null)
            return new CachedEntity<TValue?>(default, false);

        if (!redisValue.HasValue)
            return new CachedEntity<TValue?>(default, true);

        try
        {
            var deserializeObject = JsonSerializer.Deserialize<TValue>(redisValue!);

            return new CachedEntity<TValue?>(deserializeObject, true);
        }
        catch (JsonException e)
        {
            _logger.LogError($"Cant deserialize: {redisValue}. Error: ", e);
            return new CachedEntity<TValue?>(default, false);
        }
    }
}  
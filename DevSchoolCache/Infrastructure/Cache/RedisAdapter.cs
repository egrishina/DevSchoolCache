namespace DevSchoolCache;

public class RedisAdapter: IRedisAdapter
{
    public Task<bool> TryAddValueAsync<TValue>(string key, TValue value, TimeSpan? expiry = null)
    {
        throw new NotImplementedException();
    }

    public Task DeleteValueAsync(string key)
    {
        throw new NotImplementedException();
    }

    public Task<bool> TryGetValueAsync<TValue>(string key, out TValue? value)
    {
        throw new NotImplementedException();
    }
}
namespace DevSchoolCache;

public interface IRedisAdapter
{
    Task<bool> TryAddValueAsync<TValue>(string key, TValue value, TimeSpan? expiry = null);

    Task DeleteValueAsync(string key);

    Task<TValue?> TryGetValueAsync<TValue>(string key);
}
namespace DevSchoolCache;

public interface IRedisAdapter<T>
{
    public Task<T> GetOrAddAsync(string key);
}
namespace DevSchoolCache;

public interface IMemoryCacheAdapter<T>
{
    public Task<T> GetOrAddAsync(string key);
}
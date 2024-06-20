namespace DevSchoolCache;

public interface IRepository<T>
{
    public Task<T> GetOrAddAsync(string key);
}
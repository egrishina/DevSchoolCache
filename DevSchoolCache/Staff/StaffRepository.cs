using Microsoft.EntityFrameworkCore;
using Model;

namespace DevSchoolCache;

public class StaffRepository : IRepository<Staff>
{
    private IHybridCacheService<Staff> _cacheService;
    public StaffRepository(DatabaseContext context, IHybridCacheService<Staff> cacheService)
    {
        Context = context;
        _cacheService = cacheService;
        Set = Context.Set<Staff>();
    }

    private DbContext Context { get; }

    private DbSet<Staff> Set { get; }

    public IQueryable<Staff> GetAll() => Set;

    public async Task<Staff?> TryGetById(long id)
    {
        var key = FromIdToKey(id);
        return await _cacheService.GetOrAddAsync(key, () => Set.SingleOrDefault(s => s.Id == id));
    }
    
    private string FromIdToKey(long id)
    {
        return $"{typeof(Staff).Name}.{id}";
    }
}
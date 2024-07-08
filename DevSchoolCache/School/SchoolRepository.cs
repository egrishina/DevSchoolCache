using Microsoft.EntityFrameworkCore;
using Model;

namespace DevSchoolCache;

public class SchoolRepository : IRepository<School>
{
    private IHybridCacheService<School> _cacheService;
    public SchoolRepository(DatabaseContext context, IHybridCacheService<School> cacheService)
    {
        Context = context;
        _cacheService = cacheService;
        Set = Context.Set<School>();
    }


    private DbContext Context { get; }

    private DbSet<School> Set { get; }

    public IQueryable<School> GetAll() => Set;

    public async Task<School?> TryGetById(long id)
    {
        var key = FromIdToKey(id);
        return await _cacheService.GetOrAddAsync(key, () => Set.SingleOrDefault(s => s.Id == id));
    }
    
    private static string FromIdToKey(long id)
    {
        return $"{typeof(School).Name}.{id}";
    }
}
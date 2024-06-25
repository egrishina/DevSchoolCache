using Microsoft.EntityFrameworkCore;
using Model;

namespace DevSchoolCache;

public class SchoolRepository : IRepository<School>
{
    public SchoolRepository(DatabaseContext context)
    {
        Context = context;
        Set = Context.Set<School>();
    }

    private DbContext Context { get; }

    private DbSet<School> Set { get; }

    public IQueryable<School> GetAll() => Set;

    public School? TryGetById(long id) => Set
        .SingleOrDefault(s => s.Id == id);
}
using Microsoft.EntityFrameworkCore;
using Model;

namespace DevSchoolCache;

public class StaffRepository : IRepository<Staff>
{
    public StaffRepository(DatabaseContext context)
    {
        Context = context;
        Set = Context.Set<Staff>();
    }

    private DbContext Context { get; }

    private DbSet<Staff> Set { get; }

    public IQueryable<Staff> GetAll() => Set;

    public Staff? TryGetById(long id) => Set
        .SingleOrDefault(s => s.Id == id);
}
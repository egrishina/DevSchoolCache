using Microsoft.EntityFrameworkCore;

namespace DevSchoolCache;

public class StaffRepository(DbContext context) : RepositoryBase<DbContext, Staff>(context), IStaffRepository
{
    public override IQueryable<Staff> GetAll() => Query;

    public override Staff? TryGetById(long id) => Query
        .SingleOrDefault(s => s.Id == id);
}
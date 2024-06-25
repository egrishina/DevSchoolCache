using Model;

namespace DevSchoolCache;

public class StaffRepository(DatabaseContext context) : RepositoryBase<DatabaseContext, Staff>(context), IStaffRepository
{
    public override IQueryable<Staff> GetAll() => Query;

    public override Staff? TryGetById(long id) => Query
        .SingleOrDefault(s => s.Id == id);
}
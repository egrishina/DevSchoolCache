using Microsoft.EntityFrameworkCore;

namespace DevSchoolCache;

public class SchoolRepository(DbContext context) : RepositoryBase<DbContext, School>(context), ISchoolRepository
{
    public override IQueryable<School> GetAll() => Query;

    public override School? TryGetById(long id) => Query
        .SingleOrDefault(s => s.Id == id);
}
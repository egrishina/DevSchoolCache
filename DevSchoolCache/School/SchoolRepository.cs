using Model;

namespace DevSchoolCache;

public class SchoolRepository(DatabaseContext context) : RepositoryBase<DatabaseContext, School>(context), ISchoolRepository
{
    public override IQueryable<School> GetAll() => Query;

    public override School? TryGetById(long id) => Query
        .SingleOrDefault(s => s.Id == id);
}
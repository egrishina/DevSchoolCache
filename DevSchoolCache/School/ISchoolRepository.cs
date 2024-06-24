namespace DevSchoolCache;

public interface ISchoolRepository
{
    public School? TryGetById(long id);

    public IQueryable<School> GetAll();
}
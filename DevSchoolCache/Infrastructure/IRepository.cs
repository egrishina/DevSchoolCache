namespace DevSchoolCache;

public interface IRepository<TEntity>
{
    Task<TEntity?> TryGetById(long id);
    IQueryable<TEntity> GetAll();
}
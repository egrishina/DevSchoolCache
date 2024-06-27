namespace DevSchoolCache;

public interface IRepository<out TEntity>
{
    TEntity? TryGetById(long id);
    IQueryable<TEntity> GetAll();
}
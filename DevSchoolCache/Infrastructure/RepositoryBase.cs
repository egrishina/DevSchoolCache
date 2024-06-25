using Microsoft.EntityFrameworkCore;

namespace DevSchoolCache;

public abstract class RepositoryBase<TDbContext, TEntity> : IRepository<TEntity>
    where TDbContext : DbContext
    where TEntity : class
{
    protected RepositoryBase(TDbContext context)
    {
        Context = context;
        Set = Context.Set<TEntity>();
    }

    private TDbContext Context { get; }

    private DbSet<TEntity> Set { get; }

    protected virtual IQueryable<TEntity> Query => Set;

    public abstract IQueryable<TEntity> GetAll();

    public abstract TEntity? TryGetById(long id);
}

public interface IRepository<out TEntity>
{
    TEntity? TryGetById(long id);
    IQueryable<TEntity> GetAll();
}
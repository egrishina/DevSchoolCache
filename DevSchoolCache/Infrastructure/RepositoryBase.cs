using Microsoft.EntityFrameworkCore;

namespace DevSchoolCache;

public abstract class RepositoryBase<TDbContext, TEntity>
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

    public async Task CreateAsync(TEntity entity)
    {
        await Set.AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        Set.Update(entity);
        await Context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        Set.Remove(entity);
        await Context.SaveChangesAsync();
    }

    public async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
    {
        Set.RemoveRange(entities);
        await Context.SaveChangesAsync();
    }

    public Task<int> CountAsync() => Set.CountAsync();
}
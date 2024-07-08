namespace DevSchoolCache;

public record CachedEntity<TEntity>(TEntity? Entity, bool Exists);
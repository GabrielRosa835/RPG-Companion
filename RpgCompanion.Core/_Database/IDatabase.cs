namespace RpgCompanion.Core;

public interface IDatabase
{
    IQuery<T> Query<T>() where T : class, IEntity;
    Task SaveAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class, IEntity;
    void Save<T>(T entity) where T : class, IEntity;
    Task<T?> GetAsync<T>(DatabaseId<T> id, CancellationToken cancellationToken = default) where T : class, IEntity;
    T? Get<T>(DatabaseId<T> id) where T : class, IEntity;
}

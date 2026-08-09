namespace RpgCompanion.Core;

// TODO: Add delete option
// TODO: Check viability of unrestricting from IEntity

public interface IDatabase
{
    IQuery<T> Query<T>();

    T? Get<T>(DatabaseId<T> id) where T : IEntity;
    T? Get<T>(DatabaseId id) where T : IEntity;

    Task<T?> GetAsync<T>(DatabaseId<T> id, CancellationToken cancellationToken = default) where T : IEntity;
    Task<T?> GetAsync<T>(DatabaseId id, CancellationToken cancellationToken = default) where T : IEntity;

    void Save<T>(T subject);

    // void Save<T>(T entity) where T : class, IEntity;
    Task SaveAsync<T>(T entity, CancellationToken cancellationToken = default) where T : IEntity;

    T? Remove<T>(DatabaseId<T> id) where T : IEntity; // Removes and returns?
    T? Remove<T>(T subject); // Removes and returns?
    Task<T?> RemoveAsync<T>(DatabaseId<T> id, CancellationToken cancellationToken = default) where T : IEntity;
    Task<T?> RemoveAsync<T>(T subject, CancellationToken cancellationToken = default);
}

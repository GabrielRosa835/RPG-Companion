namespace RpgCompanion.Core;

public interface IDatabase
{
    IQuery<T> Query<T>() where T : class, IEntity;
    Task SaveAsync<T>(T entity) where T : class, IEntity;
    Task<T?> GetAsync<T>(DatabaseId<T> id) where T : class, IEntity;
}

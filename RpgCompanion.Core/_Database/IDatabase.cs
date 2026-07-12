namespace RpgCompanion.Core;

public interface IDatabase
{
    IQuery<T> Set<T>() where T : class, IEntity<T>;
    Task SaveAsync<T>(T document) where T : class, IEntity<T>;
    Task<T?> GetAsync<T>(DatabaseId<T> id) where T : class, IEntity<T>;

}

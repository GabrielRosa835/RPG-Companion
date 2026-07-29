namespace RpgCompanion.Host.Database;

using MongoDB.Driver;

public class MongoDatabase(
    IMongoDatabase _database)
    : IDatabase
{
    internal static string GetCollectionName<T>() => typeof(T).FullName!.ToLowerInvariant().Replace('.', '_');

    internal IMongoCollection<T> GetCollection<T>() where T : class, IEntity
    {
        return _database.GetCollection<T>(GetCollectionName<T>());
    }

    public IQuery<T> Query<T>() where T : class, IEntity
    {
        return new MongoQuery<T>(this, GetCollection<T>());
    }

    public void Save<T>(T entity) where T : class, IEntity
    {
        var collection = GetCollection<T>();

        // Assuming DbId maps down to a string "_id" via MongoDB conventions/serializers
        var filter = Builders<T>.Filter.Eq(e => e.DbId, entity.DbId);
        collection.ReplaceOne(filter, entity, new ReplaceOptions { IsUpsert = true });
    }

    public Task SaveAsync<T>(T entity, CancellationToken cancellationToken) where T : class, IEntity
    {
        var collection = GetCollection<T>();

        // Assuming DbId maps down to a string "_id" via MongoDB conventions/serializers
        var filter = Builders<T>.Filter.Eq(e => e.DbId, entity.DbId);
        return collection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = true }, cancellationToken);
    }

    public T? Get<T>(DatabaseId<T> id) where T : class, IEntity
    {
        var collection = GetCollection<T>();
        var filter = Builders<T>.Filter.Eq<DatabaseId>(e => e.DbId, id);
        return collection.Find(filter).FirstOrDefault();
    }

    public Task<T?> GetAsync<T>(DatabaseId<T> id, CancellationToken cancellationToken) where T : class, IEntity
    {
        var collection = GetCollection<T>();
        var filter = Builders<T>.Filter.Eq<DatabaseId>(e => e.DbId, id);
        return collection.Find(filter).FirstOrDefaultAsync(cancellationToken)!;
    }
}

namespace RpgCompanion.Host.Database;

using MongoDB.Driver;

public class MongoDatabase : IDatabase
{
    private readonly IMongoDatabase _database;

    public MongoDatabase(IMongoDatabase database)
    {
        _database = database;
    }
    
    internal static string GetCollectionName<T>() => typeof(T).FullName!.ToLowerInvariant().Replace('.', '_');

    internal IMongoCollection<T> GetCollection<T>() where T : class, IEntity
    {
        return _database.GetCollection<T>(GetCollectionName<T>());
    }

    public IQuery<T> Query<T>() where T : class, IEntity
    {
        return new MongoQuery<T>(this, GetCollection<T>());
    }

    public async Task SaveAsync<T>(T entity) where T : class, IEntity
    {
        var collection = GetCollection<T>();

        // Assuming DbId maps down to a string "_id" via MongoDB conventions/serializers
        var filter = Builders<T>.Filter.Eq(e => e.DbId, entity.DbId);
        await collection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = true });
    }

    public async Task<T?> GetAsync<T>(DatabaseId<T> id) where T : class, IEntity
    {
        var collection = GetCollection<T>();
        var filter = Builders<T>.Filter.Eq<DatabaseId>(e => e.DbId, id);
        return await collection.Find(filter).FirstOrDefaultAsync();
    }
}

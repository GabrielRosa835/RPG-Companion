namespace RpgCompanion.Host.Database;

using MongoDB.Driver;

public class MongoDatabase : IDatabase
{
    private readonly IMongoDatabase _database;

    public MongoDatabase(IMongoDatabase database)
    {
        _database = database;
    }

    // A helper to generate safe, collision-free collection names
    internal static string GetCollectionName<T>() => typeof(T).FullName!.ToLowerInvariant().Replace('.', '_');

    internal IMongoCollection<T> GetCollection<T>() where T : class, IEntity<T>
    {
        return _database.GetCollection<T>(GetCollectionName<T>());
    }

    public IQuery<T> Set<T>() where T : class, IEntity<T>
    {
        return new MongoQuery<T>(this, GetCollection<T>());
    }

    public async Task SaveAsync<T>(T document) where T : class, IEntity<T>
    {
        var collection = GetCollection<T>();

        // Assuming DbId maps down to a string "_id" via MongoDB conventions/serializers
        var filter = Builders<T>.Filter.Eq(e => e.DbId, document.DbId);
        await collection.ReplaceOneAsync(filter, document, new ReplaceOptions { IsUpsert = true });
    }

    public async Task<T?> GetAsync<T>(DatabaseId<T> id) where T : class, IEntity<T>
    {
        var collection = GetCollection<T>();
        var filter = Builders<T>.Filter.Eq(e => e.DbId, id);
        return await collection.Find(filter).FirstOrDefaultAsync();
    }
}

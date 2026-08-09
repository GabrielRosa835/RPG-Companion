namespace RpgCompanion.Host.Persistence.Storage;

using LiteDB;
using RpgCompanion.Toolbox;

public class LiteDbDynamicStorage : IStorage, IDisposable
{
    private readonly ILiteDatabase _database;

    /// <summary>
    /// Initializes the LiteDB storage.
    ///
    /// Connection String Examples:
    /// - Basic local file: "Filename=PluginData.db;"
    /// - Shared mode (allows concurrent read/write from different processes, great for debugging): "Filename=PluginData.db;Connection=Shared;"
    /// - In-Memory (useful for testing): "Filename=:memory:;"
    /// </summary>
    public LiteDbDynamicStorage(string connectionString)
    {
        _database = new LiteDatabase(connectionString);
    }

    // Envelope to guarantee an _id for LiteDB, allowing storage of primitives and complex objects alike
    private class StorageEnvelope<T>
    {
        [BsonId]
        public string Id { get; set; } = string.Empty;
        public T? Value { get; set; }
    }

    private ILiteCollection<StorageEnvelope<T>> GetCollection<T>()
    {
        // Remember to prefix this with PluginId in the future to avoid cross-plugin collisions!
        return _database.GetCollection<StorageEnvelope<T>>(typeof(T).Name);
    }

    public bool Contains<T>(StorageKey<T> key)
    {
        // Translates to an efficient primary key index seek in LiteDB
        return GetCollection<T>().Exists(x => x.Id == key.Value);
    }

    public bool Add<T>(StorageKey<T> key, T value)
    {
        var collection = GetCollection<T>();

        if (collection.Exists(x => x.Id == key.Value))
        {
            return false;
        }

        var envelope = new StorageEnvelope<T> { Id = key.Value, Value = value };

        try
        {
            collection.Insert(envelope);
            return true;
        }
        catch (LiteException ex) when (ex.ErrorCode == LiteException.INDEX_DUPLICATE_KEY)
        {
            // Catches the edge case where another thread/process inserted it between our check and insert
            return false;
        }
    }

    public bool Remove<T>(StorageKey<T> key)
    {
        // LiteDB's Delete conveniently returns true if a document was actually found and deleted
        return GetCollection<T>().Delete(key.Value);
    }

    public T? Put<T>(StorageKey<T> key, T value)
    {
        var collection = GetCollection<T>();

        // Fetch the previous element to fulfill the interface contract
        var previous = collection.FindById(key.Value);

        var envelope = new StorageEnvelope<T> { Id = key.Value, Value = value };

        // Upsert inserts if it doesn't exist, updates if it does
        collection.Upsert(envelope);

        return previous is not null ? previous.Value : default;
    }

    public T Get<T>(StorageKey<T> key)
    {
        var envelope = GetCollection<T>().FindById(key.Value);

        if (envelope is null)
        {
            throw new KeyNotFoundException($"Key '{key.Value}' not found in LiteDB storage.");
        }

        return envelope.Value!;
    }

    public T? GetOrDefault<T>(StorageKey<T> key)
    {
        var envelope = GetCollection<T>().FindById(key.Value);
        return envelope is not null ? envelope.Value : default;
    }

    public T Acquire<T>(StorageKey<T> key)
    {
        var collection = GetCollection<T>();
        var envelope = collection.FindById(key.Value);

        if (envelope is null)
        {
            throw new KeyNotFoundException($"Key '{key.Value}' not found in LiteDB storage.");
        }

        collection.Delete(key.Value);
        return envelope.Value!;
    }

    public T? AcquireOrDefault<T>(StorageKey<T> key)
    {
        var collection = GetCollection<T>();
        var envelope = collection.FindById(key.Value);

        if (envelope is null)
        {
            return default;
        }

        collection.Delete(key.Value);
        return envelope.Value;
    }

    public void Dispose()
    {
        _database.Dispose();
    }
}

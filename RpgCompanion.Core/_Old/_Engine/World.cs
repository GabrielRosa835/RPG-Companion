namespace RpgCompanion.Events;

using Toolbox;

public class World : IStorage
{
    public required PluginKey Plugin { get; init; }

    private readonly IStorage _storage = new ConcurrentDynamicStorage();

    public void Add<T>(StorageKey<T> storageKey, T value) => _storage.Add(storageKey, value);
    public void Put<T>(StorageKey<T> storageKey, T value) => _storage.Put(storageKey, value);
    public void Remove<T>(StorageKey<T> storageKey) => _storage.Remove(storageKey);
    public T Get<T>(StorageKey<T> storageKey) => _storage.Get(storageKey);
    public T? GetOrDefault<T>(StorageKey<T> storageKey) => _storage.GetOrDefault(storageKey);
    public T Acquire<T>(StorageKey<T> storageKey) => _storage.Acquire(storageKey);
    public T? AcquireOrDefault<T>(StorageKey<T> storageKey) => _storage.AcquireOrDefault(storageKey);
}

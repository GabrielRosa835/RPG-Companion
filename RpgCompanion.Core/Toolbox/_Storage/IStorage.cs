namespace RpgCompanion.Core.Toolbox;

public interface IStorage : IEnumerable<(string Key, object? Value)>
{
    void Add<T>(StorageKey storageKey, T value);
    void Add<T>(StorageKey<T> storageKey, T value);
    void Remove<T>(StorageKey storageKey);
    void Remove<T>(StorageKey<T> storageKey);
    void Remove(StorageKey groupStorageKey);
    void RemoveRange(params IEnumerable<StorageKey> keys);
    void RemoveRange<T>(params IEnumerable<StorageKey<T>> keys);
    T Get<T>(StorageKey storageKey);
    T Get<T>(StorageKey<T> storageKey);
    T? GetOrDefault<T>(StorageKey storageKey);
    T? GetOrDefault<T>(StorageKey<T> storageKey);
}

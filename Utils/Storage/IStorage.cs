namespace Utils;

using Storage;

public interface IStorage : IEnumerable<(string Key, object? Value)>
{
    void Add<T>(Key key, T value);
    void Add<T>(Key<T> key, T value);
    void Remove<T>(Key key);
    void Remove<T>(Key<T> key);
    void Remove(Key groupKey);
    void RemoveRange(params IEnumerable<Key> keys);
    void RemoveRange<T>(params IEnumerable<Key<T>> keys);
    T Get<T>(Key key);
    T Get<T>(Key<T> key);
    T? GetOrDefault<T>(Key key);
    T? GetOrDefault<T>(Key<T> key);
}

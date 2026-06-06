namespace RpgCompanion.Core.Toolbox;

using System.Collections;
using System.Collections.Concurrent;

public class ConcurrentDynamicStorage : IStorage
{
    private readonly ConcurrentDictionary<(string Name, Type Type), object?> _values = new();

    public void Add<T>(StorageKey storageKey, T value) => AddInternal(storageKey.Value, value);
    public void Add<T>(StorageKey<T> storageKey, T value) => AddInternal(storageKey.Value, value);

    public void Remove<T>(StorageKey storageKey) => RemoveInternal(storageKey.Value, typeof(T));
    public void Remove<T>(StorageKey<T> storageKey) => RemoveInternal(storageKey.Value, typeof(T));

    public void Remove(StorageKey groupStorageKey)
    {
        var keysToRemove = _values.Keys.Where(k => k.Name == groupStorageKey.Value).ToList();
        foreach (var key in keysToRemove)
        {
            _values.TryRemove(key, out _);
        }
    }

    public void RemoveRange(params IEnumerable<StorageKey> keys)
    {
        var keysArray = keys.Select(k => k.Value).ToArray();
        var keysToRemove = _values.Keys.Where(k => keysArray.Contains(k.Name)).ToList();

        foreach (var key in keysToRemove)
        {
            _values.TryRemove(key, out _);
        }
    }

    public void RemoveRange<T>(params IEnumerable<StorageKey<T>> keys)
    {
        var type = typeof(T);
        foreach (StorageKey<T> key in keys)
        {
            RemoveInternal(key.Value, type);
        }
    }

    public T Get<T>(StorageKey storageKey) => GetUnsafeInternal<T>(storageKey.Value);
    public T Get<T>(StorageKey<T> storageKey) => GetUnsafeInternal<T>(storageKey.Value);

    public T? GetOrDefault<T>(StorageKey storageKey) => GetOrDefaultInternal<T>(storageKey.Value);
    public T? GetOrDefault<T>(StorageKey<T> storageKey) => GetOrDefaultInternal<T>(storageKey.Value);

    private void AddInternal<T>(string key, T value)
    {
        _values.AddOrUpdate((key, typeof(T)), value, (_, _) => value);
    }

    private void RemoveInternal(string key, Type valueType)
    {
        _values.TryRemove((key, valueType), out _);
    }

    private T GetUnsafeInternal<T>(string key)
    {
        if (_values.TryGetValue((key, typeof(T)), out var value))
        {
            if (value is null)
            {
                return (T) value!;
            }
            if (value is T typedValue)
            {
                return typedValue;
            }
            const string typeMsg = "Stored value with key '{0}' is not of the asked type, but is '{1}'";
            string typeMsgFormatted = string.Format(typeMsg, key, value.GetType().Name);
            throw new InvalidOperationException(typeMsgFormatted);
        }
        const string keyMsg = "Key '{0}' not found in dictionary";
        string keyMsgFormatted = string.Format(keyMsg, key);
        throw new KeyNotFoundException(keyMsgFormatted);
    }

    private T? GetOrDefaultInternal<T>(string key)
    {
        if (_values.TryGetValue((key, typeof(T)), out var value))
        {
            if (value is null)
            {
                return default;
            }
            if (value is T typedValue)
            {
                return typedValue;
            }
        }

        return default;
    }

    public IEnumerator<(string Key, object? Value)> GetEnumerator()
        => _values.Select(kvp => (kvp.Key.Name, kvp.Value)).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

namespace RpgCompanion.Core.Toolbox;

using System.Collections;

public class SimpleStorage : IStorage
{
    private readonly Dictionary<string, object?> _values = [];
    public void Add<T>(StorageKey storageKey, T value) => AddInternal(storageKey.Value, value);
    public void Add<T>(StorageKey<T> storageKey, T value) => AddInternal(storageKey.Value, value);
    public void Remove<T>(StorageKey storageKey) => RemoveInternal(storageKey.Value);
    public void Remove<T>(StorageKey<T> storageKey) => RemoveInternal(storageKey.Value);

    public void Remove(StorageKey groupStorageKey)
    {
        var keysToRemove = _values
            .Where(kvp => kvp.Key == groupStorageKey.Value)
            .Select(kvp => kvp.Key)
            .ToList();
        foreach (var key in keysToRemove)
        {
            RemoveInternal(key);
        }
    }

    public void RemoveRange(params IEnumerable<StorageKey> keys)
    {
        var keysArray = keys.ToArray();
        foreach (var key in _values.Where(kvp => keysArray.Contains(kvp.Key)).Select(kvp => kvp.Key))
        {
            RemoveInternal(key);
        }
    }

    public void RemoveRange<T>(params IEnumerable<StorageKey<T>> keys)
    {
        foreach (StorageKey<T> key in keys)
        {
            RemoveInternal(key.Value);
        }
    }

    public T Get<T>(StorageKey storageKey) => GetUnsafeInternal<T>(storageKey.Value);
    public T Get<T>(StorageKey<T> storageKey) => GetUnsafeInternal<T>(storageKey.Value);
    public T? GetOrDefault<T>(StorageKey storageKey) => GetOrDefaultInternal<T>(storageKey.Value);
    public T? GetOrDefault<T>(StorageKey<T> storageKey) => GetOrDefaultInternal<T>(storageKey.Value);

    private void AddInternal<T>(string key, T value)
    {
        _values[key] = value;
    }

    private void RemoveInternal(string key)
    {
        _values.Remove(key);
    }

    private T GetUnsafeInternal<T>(string key)
    {
        if (_values.TryGetValue(key, out var value))
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
            string typeMsgFormatted = string.Format(typeMsg, key, value?.GetType().Name ?? "null");
            throw new InvalidOperationException(typeMsgFormatted);
        }
        const string keyMsg = "Key '{0}' not found in dictionary";
        string keyMsgFormatted = string.Format(keyMsg, key);
        throw new KeyNotFoundException(keyMsgFormatted);
    }

    private T? GetOrDefaultInternal<T>(string key)
    {
        if (_values.TryGetValue(key, out var value))
        {
            if (value is null)
            {
                return (T) value!;
            }
            if (value is T typedValue)
            {
                return typedValue;
            }
        }

        return default;
    }

    public IEnumerator<(string Key, object? Value)> GetEnumerator()
        => _values.Select(kvp => (kvp.Key, kvp.Value)).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

namespace Utils.Map;

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Storage;

/// <summary>
/// A thread-safe dynamic map utility that provides flexible key-value storage.
/// </summary>
public class ConcurrentDynamicStorage : IStorage
{
    private readonly ConcurrentDictionary<(string Name, Type Type), object?> _values = new();

    public void Add<T>(Key key, T value) => AddInternal(key.Value, value);
    public void Add<T>(Key<T> key, T value) => AddInternal(key.Value, value);

    public void Remove<T>(Key key) => RemoveInternal(key.Value, typeof(T));
    public void Remove<T>(Key<T> key) => RemoveInternal(key.Value, typeof(T));

    public void Remove(Key groupKey)
    {
        // Extract keys to a list first to avoid holding up the dictionary's internal locks
        // while performing multiple removals.
        var keysToRemove = _values.Keys.Where(k => k.Name == groupKey.Value).ToList();
        foreach (var key in keysToRemove)
        {
            _values.TryRemove(key, out _);
        }
    }

    public void RemoveRange(params IEnumerable<Key> keys)
    {
        var keysArray = keys.Select(k => k.Value).ToArray();
        var keysToRemove = _values.Keys.Where(k => keysArray.Contains(k.Name)).ToList();

        foreach (var key in keysToRemove)
        {
            _values.TryRemove(key, out _);
        }
    }

    public void RemoveRange<T>(params IEnumerable<Key<T>> keys)
    {
        var type = typeof(T);
        foreach (Key<T> key in keys)
        {
            RemoveInternal(key.Value, type);
        }
    }

    public T Get<T>(Key key) => GetUnsafeInternal<T>(key.Value);
    public T Get<T>(Key<T> key) => GetUnsafeInternal<T>(key.Value);

    public T? GetOrDefault<T>(Key key) => GetOrDefaultInternal<T>(key.Value);
    public T? GetOrDefault<T>(Key<T> key) => GetOrDefaultInternal<T>(key.Value);

    private void AddInternal<T>(string key, T value)
    {
        // AddOrUpdate ensures thread safety when adding or replacing a value
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

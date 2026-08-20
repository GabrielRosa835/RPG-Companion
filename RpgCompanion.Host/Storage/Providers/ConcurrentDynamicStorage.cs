namespace RpgCompanion.Host;

using System.Collections.Concurrent;

public class ConcurrentDynamicStorage : IStorage
{
    private readonly ConcurrentDictionary<(string Name, Type Type), object?> _values = new();
    private static (string Name, Type Type) Key<T>(StorageKey<T> key) => (key.Value, typeof(T));

    public bool Contains<T>(StorageKey<T> key)
    {
        return _values.ContainsKey(Key(key));
    }

    public bool Add<T>(StorageKey<T> key, T value)
    {
        return _values.TryAdd(Key(key), value);
    }

    public T? Put<T>(StorageKey<T> key, T value)
    {
        T? original = GetOrDefault(key);
        _values[Key(key)] = value;
        return original;
    }

    public bool Remove<T>(StorageKey<T> key)
    {
        return _values.TryRemove((key.Value, typeof(T)), out _);
    }

    public T Get<T>(StorageKey<T> key)
    {
        if (!_values.TryGetValue(Key(key), out var value))
        {
            const string keyMsg = "Key '{0}' not found in dictionary";
            string keyMsgFormatted = string.Format(keyMsg, key.Value);
            throw new KeyNotFoundException(keyMsgFormatted);
        }
        if (value is null)
        {
            return (T) value!;
        }
        if (value is T typedValue)
        {
            return typedValue;
        }
        const string typeMsg = "Stored value with key '{0}' is not of the asked type, but is '{1}'";
        string typeMsgFormatted = string.Format(typeMsg, key.Value, value.GetType().Name);
        throw new InvalidOperationException(typeMsgFormatted);
    }

    public T? GetOrDefault<T>(StorageKey<T> key)
    {
        if (!_values.TryGetValue(Key(key), out var value))
        {
            return default;
        }
        return value switch
        {
            null => default,
            T typedValue => typedValue,
            _ => default
        };
    }

    public T Acquire<T>(StorageKey<T> key)
    {
        if (!_values.TryGetValue(Key(key), out var value))
        {
            const string keyMsg = "Key '{0}' not found in dictionary";
            string keyMsgFormatted = string.Format(keyMsg, key.Value);
            throw new KeyNotFoundException(keyMsgFormatted);
        }
        if (value is null)
        {
            Remove(key);
            return (T) value!;
        }
        if (value is T typedValue)
        {
            Remove(key);
            return typedValue;
        }
        const string typeMsg = "Stored value with key '{0}' is not of the asked type, but is '{1}'";
        string typeMsgFormatted = string.Format(typeMsg, key.Value, value.GetType().Name);
        throw new InvalidOperationException(typeMsgFormatted);
    }

    public T? AcquireOrDefault<T>(StorageKey<T> key)
    {
        if (!_values.TryGetValue(Key(key), out var value))
        {
            return default;
        }
        Remove(key);
        return value switch
        {
            null => default,
            T typedValue => typedValue,
            _ => default
        };
    }
}

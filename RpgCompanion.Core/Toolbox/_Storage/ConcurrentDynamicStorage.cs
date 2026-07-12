namespace RpgCompanion.Toolbox;

using System.Collections;
using System.Collections.Concurrent;

public class ConcurrentDynamicStorage : IStorage
{
    private readonly ConcurrentDictionary<(string Name, Type Type), object?> _values = new();

    public void Add<T>(StorageKey<T> storageKey, T value)
    {
        _values.AddOrUpdate((storageKey.Value, typeof(T)), value, (_, v) => v);
    }

    public void Put<T>(StorageKey<T> storageKey, T value)
    {
        _values.AddOrUpdate((storageKey.Value, typeof(T)), value, (_, _) => value);
    }

    public void Remove<T>(StorageKey<T> storageKey)
    {
        _values.TryRemove((storageKey.Value, typeof(T)), out _);
    }

    public T Get<T>(StorageKey<T> storageKey)
    {
        if (_values.TryGetValue((storageKey.Value, typeof(T)), out var value))
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
            string typeMsgFormatted = string.Format(typeMsg, storageKey.Value, value.GetType().Name);
            throw new InvalidOperationException(typeMsgFormatted);
        }
        const string keyMsg = "Key '{0}' not found in dictionary";
        string keyMsgFormatted = string.Format(keyMsg, storageKey.Value);
        throw new KeyNotFoundException(keyMsgFormatted);
    }

    public T? GetOrDefault<T>(StorageKey<T> storageKey)
    {
        if (!_values.TryGetValue((storageKey.Value, typeof(T)), out var value))
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

    public T Acquire<T>(StorageKey<T> storageKey)
    {
        if (_values.TryGetValue((storageKey.Value, typeof(T)), out var value))
        {
            if (value is null)
            {
                Remove(storageKey);
                return (T) value!;
            }
            if (value is T typedValue)
            {
                Remove(storageKey);
                return typedValue;
            }
            const string typeMsg = "Stored value with key '{0}' is not of the asked type, but is '{1}'";
            string typeMsgFormatted = string.Format(typeMsg, storageKey.Value, value.GetType().Name);
            throw new InvalidOperationException(typeMsgFormatted);
        }
        const string keyMsg = "Key '{0}' not found in dictionary";
        string keyMsgFormatted = string.Format(keyMsg, storageKey.Value);
        throw new KeyNotFoundException(keyMsgFormatted);
    }

    public T? AcquireOrDefault<T>(StorageKey<T> storageKey)
    {
        if (!_values.TryGetValue((storageKey.Value, typeof(T)), out var value))
        {
            return default;
        }
        Remove(storageKey);
        return value switch
        {
            null => default,
            T typedValue => typedValue,
            _ => default
        };
    }
}

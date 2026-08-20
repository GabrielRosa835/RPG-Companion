namespace RpgCompanion.Host;

using Toolbox;

public class SimpleStorage : IStorage
{
    private readonly Dictionary<string, object?> _values = [];

    public bool Contains<T>(StorageKey<T> key)
    {
        return _values.ContainsKey(key.Value);
    }

    public bool Add<T>(StorageKey<T> key, T value)
    {
        return _values.TryAdd(key.Value, value);
    }

    public T? Put<T>(StorageKey<T> key, T value)
    {
        T? original = GetOrDefault(key);
        _values[key.Value] = value;
        return original;
    }

    public bool Remove<T>(StorageKey<T> key)
    {
        return _values.Remove(key.Value);
    }

    public T Get<T>(StorageKey<T> key)
    {
        if (!_values.TryGetValue(key.Value, out var value))
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
        string typeMsgFormatted = string.Format(typeMsg, key.Value, value?.GetType().Name ?? "null");
        throw new InvalidOperationException(typeMsgFormatted);
    }

    public T? GetOrDefault<T>(StorageKey<T> key)
    {
        if (!_values.TryGetValue(key.Value, out var value))
        {
            return default;
        }
        return value switch
        {
            null => (T) value!,
            T typedValue => typedValue,
            _ => default
        };
    }

    public T Acquire<T>(StorageKey<T> key)
    {
        var value = Get(key);
        Remove(key);
        return value;
    }

    public T? AcquireOrDefault<T>(StorageKey<T> key)
    {
        var value = GetOrDefault(key);
        Remove(key);
        return value;
    }
}

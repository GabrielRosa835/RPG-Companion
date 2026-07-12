namespace RpgCompanion.Toolbox;

using System.Collections;

public class DynamicStorage : IStorage
{
    private readonly Dictionary<(string Name, Type Type), object?> _values = [];

    public void Add<T>(StorageKey<T> storageKey, T value)
    {
        if (_values.ContainsKey((storageKey.Value, typeof(T)))) return;
        _values[(storageKey.Value, typeof(T))] = value;
    }

    public void Put<T>(StorageKey<T> storageKey, T value)
    {
        _values[(storageKey.Value, typeof(T))] = value;
    }

    public void Remove<T>(StorageKey<T> storageKey)
    {
        _values.Remove((storageKey.Value, typeof(T)));
    }

    public T Get<T>(StorageKey<T> storageKey)
    {
        if (_values.TryGetValue((storageKey.Value, typeof(T)), out var value))
        {
            if (value is null) return (T) value!;
            if (value is T typedValue) return typedValue;
            const string typeMsg = "Stored value with key '{0}' is not of the asked type, but is '{1}'";
            string typeMsgFormatted = string.Format(typeMsg, storageKey.Value, value?.GetType().Name ?? "null");
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
            null => (T) value!,
            T typedValue => typedValue,
            _ => default
        };
    }

    public T Acquire<T>(StorageKey<T> storageKey)
    {
        var value = Get(storageKey);;
        Remove(storageKey);
        return value;
    }

    public T? AcquireOrDefault<T>(StorageKey<T> storageKey)
    {
        var value = GetOrDefault(storageKey);
        Remove(storageKey);
        return value;
    }
}

namespace Utils.Storage;

using System.Collections;

/// <summary>
/// A dynamic map utility that provides flexible key-value storage with strong typing.
/// Supports both untyped and strongly-typed key access patterns.
/// </summary>
public class DynamicStorage : IStorage
{
    private readonly Dictionary<(string Name, Type Type), object?> _values = [];

    /// <summary>
    /// Adds or updates a key-value pair in the map using an untyped Key.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="key">The Key instance.</param>
    /// <param name="value">The value to store.</param>
    /// <returns>The current Map instance for method chaining.</returns>
    public void Add<T>(Key key, T value) => AddInternal(key.Value, value);

    /// <summary>
    /// Adds or updates a key-value pair in the map using a strongly-typed Key.
    /// </summary>
    /// <typeparam name="T">The type of the value and key.</typeparam>
    /// <param name="key">The strongly-typed Key instance.</param>
    /// <param name="value">The value to store.</param>
    /// <returns>The current Map instance for method chaining.</returns>
    public void Add<T>(Key<T> key, T value) => AddInternal(key.Value, value);

    /// <summary>
    /// Removes a key-value pair from the map using an untyped Key.
    /// </summary>
    /// <param name="key">The Key instance to remove.</param>
    /// <returns>The current Map instance for method chaining.</returns>
    public void Remove<T>(Key key) => RemoveInternal(key.Value, typeof(T));

    /// <summary>
    /// Removes a key-value pair from the map using a strongly-typed Key.
    /// </summary>
    /// <typeparam name="T">The type of the key.</typeparam>
    /// <param name="key">The strongly-typed Key instance to remove.</param>
    /// <returns>The current Map instance for method chaining.</returns>
    public void Remove<T>(Key<T> key) => RemoveInternal(key.Value, typeof(T));

    /// <summary>
    /// Removes key-value pairs of all types from the map.
    /// </summary>
    /// <param name="groupKey"> The untyped key whose variations should be removed.</param>
    /// <returns>The current Map instance for method chaining.</returns>
    public void Remove(Key groupKey)
    {
        var keysToRemove = _values
            .Where(kvp => kvp.Key.Name == groupKey.Value)
            .Select(kvp => kvp.Key)
            .ToList();
        foreach (var key in keysToRemove)
        {
            RemoveInternal(key.Name, key.Type);
        }
    }

    /// <summary>
    /// Removes multiple key-value pairs of all types from the map.
    /// </summary>
    /// <param name="keys">The collection of untyped keys to remove.</param>
    /// <returns>The current Map instance for method chaining.</returns>
    public void RemoveRange(params IEnumerable<Key> keys)
    {
        var keysArray = keys.ToArray();
        foreach (var key in _values.Where(kvp => keysArray.Contains(kvp.Key.Name)).Select(kvp => kvp.Key))
        {
            RemoveInternal(key.Name, key.Type);
        }
    }

    /// <summary>
    /// Removes multiple key-value pairs of the same type from the map.
    /// </summary>
    /// <param name="keys">The collection of typed keys to remove.</param>
    /// <typeparam name="T">The type of the values.</typeparam>
    /// <returns>The current Map instance for method chaining.</returns>
    public void RemoveRange<T>(params IEnumerable<Key<T>> keys)
    {
        var type = typeof(T);
        foreach (Key<T> key in keys)
        {
            RemoveInternal(key.Value, type);
        }
    }

    /// <summary>
    /// Retrieves a value from the map using an untyped Key.
    /// This is an unsafe operation that assumes the key exists and the value matches the requested type.
    /// </summary>
    /// <typeparam name="T">The expected type of the value.</typeparam>
    /// <param name="key">The untyped Key instance to lookup.</param>
    /// <returns>The value associated with the key, cast to <typeparamref name="T"/>.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the specified key is not present in the map.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the value is found, but cannot be cast to <typeparamref name="T"/>.</exception>
    public T Get<T>(Key key) => GetUnsafeInternal<T>(key.Value);

    /// <summary>
    /// Retrieves a value from the map using a strongly-typed Key.
    /// This is an unsafe operation that assumes the key exists and the value matches the requested type.
    /// </summary>
    /// <typeparam name="T">The expected type of the value, enforced by the strongly-typed key.</typeparam>
    /// <param name="key">The strongly-typed Key instance to lookup.</param>
    /// <returns>The value associated with the key, cast to <typeparamref name="T"/>.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the specified key is not present in the map.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the value is found, but cannot be cast to <typeparamref name="T"/>.</exception>
    public T Get<T>(Key<T> key) => GetUnsafeInternal<T>(key.Value);

    /// <summary>
    /// Safely attempts to retrieve a value from the map using an untyped Key.
    /// </summary>
    /// <typeparam name="T">The expected type of the value.</typeparam>
    /// <param name="key">The untyped Key instance to lookup.</param>
    /// <returns>The casted value if the key exists and the type matches; otherwise, the default value for <typeparamref name="T"/>.</returns>
    public T? GetOrDefault<T>(Key key) => GetOrDefaultInternal<T>(key.Value);

    /// <summary>
    /// Safely attempts to retrieve a value from the map using a strongly-typed Key.
    /// </summary>
    /// <typeparam name="T">The expected type of the value, enforced by the strongly-typed key.</typeparam>
    /// <param name="key">The strongly-typed Key instance to lookup.</param>
    /// <returns>The casted value if the key exists and the type matches; otherwise, the default value for <typeparamref name="T"/>.</returns>
    public T? GetOrDefault<T>(Key<T> key) => GetOrDefaultInternal<T>(key.Value);

    private void AddInternal<T>(string key, T value)
    {
        _values[(key, typeof(T))] = value;
    }
    private void RemoveInternal(string key, Type valueType)
    {
        _values.Remove((key, valueType));
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
            string typeMsgFormatted = string.Format(typeMsg, key, value?.GetType().Name ?? "null");
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
        => _values.Select(kvp => (kvp.Key.Name, kvp.Value)).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

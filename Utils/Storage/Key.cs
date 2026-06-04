namespace Utils.Storage;

/// <summary>
/// Represents an untyped key for the map with implicit string conversion.
/// </summary>
/// <param name="Value">The string value of the key.</param>
public readonly record struct Key(string Value)
{
    /// <summary>
    /// Implicitly converts a string to a Key.
    /// </summary>
    /// <param name="value">The string value.</param>
    public static implicit operator Key(string value) => new(value);
    public static implicit operator string(Key key) => key.Value;

    /// <summary>
    /// Creates a new untyped Key instance.
    /// </summary>
    /// <param name="value">The string value of the key.</param>
    /// <returns>A new Key instance.</returns>
    public static Key Of(string value) => new(value);

    /// <summary>
    /// Creates a new untyped Key instance with type hint.
    /// </summary>
    /// <typeparam name="T">The type hint for the key.</typeparam>
    /// <param name="value">The string value of the key.</param>
    /// <returns>A new Key instance.</returns>
    public static Key<T> Of<T>(string value) => new(value);

    public Key<T> For<T>() => new(Value);
}

/// <summary>
/// Represents a strongly-typed key for the map that enforces type safety.
/// </summary>
/// <typeparam name="T">The type associated with this key.</typeparam>
/// <param name="Value">The string value of the key.</param>
public readonly record struct Key<T>(string Value)
{
    public static implicit operator Key<T>(string value) => new(value);
    public static implicit operator string(Key<T> key) => key.Value;

    public static implicit operator Key<T>(Key key) => new(key.Value);
}

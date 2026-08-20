namespace RpgCompanion.Toolbox;

public record Lazy<T>
{
    private readonly Func<T> _factory;
    private T _value;
    private bool _initialized;

    public T Value => GetValue();

    private T GetValue()
    {
        if (_initialized) return _value;
        _initialized = true;
        _value = _factory();
        return _value;
    }

    public Lazy(T value)
    {
        _factory = () => value;
        _value = value;
        _initialized = true;
    }

    public Lazy(Func<T> factory)
    {
        _factory = factory;
        _value = default!;
        _initialized = false;
    }

    public Lazy<U> Map<U>(Func<T, U> mapper) => new(() => mapper(Value));
    public Lazy<U> FlatMap<U>(Func<T, Lazy<U>> mapper) => new(() => mapper(Value).Value);
    public Lazy<T> ForceEvaluation() => new(Value);

    public static implicit operator Lazy<T>(T value) => new(value);
    public static implicit operator Lazy<T>(Func<T> factory) => new(factory);
    public static implicit operator T(Lazy<T> lazy) => lazy.Value;
}

public static class Lazy
{
    public static Lazy<T> Of<T>(Func<T> factory) => new(factory);
    public static Lazy<T> Of<T>(T value) => new(value);
}

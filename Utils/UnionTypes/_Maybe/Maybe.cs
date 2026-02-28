namespace Utils.UnionTypes;

public readonly struct Maybe<T> : IEquatable<Maybe<T>>
{
    private readonly T _value;
    private readonly bool _hasValue;

    public bool IsPresent => _hasValue;

    public bool IsEmpty => !_hasValue;

    private Maybe(T value, bool hasValue)
    {
        _value = value;
        _hasValue = hasValue;
    }

    public static Maybe<T> Some(T value) => new(value, true);
    public static Maybe<T> None() => new(default!, false);
    public static Maybe<T> From(T? value) => value is not null ? Some(value) : None();

    public static implicit operator Maybe<T>(T? value) => From(value);
    public static implicit operator Maybe<T>(Unit _) => None();
    public static implicit operator bool(Maybe<T> maybe) => maybe.IsPresent;

    public TResult Either<TResult>(Func<T, TResult> onSome, Func<TResult> onNone) => _hasValue ? onSome(_value) : onNone();

    public bool Equals(Maybe<T> other)
    {
        return _hasValue == other._hasValue && EqualityComparer<T>.Default.Equals(_value, other._value);
    }
}
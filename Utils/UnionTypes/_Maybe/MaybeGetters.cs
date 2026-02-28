namespace Utils.UnionTypes;

public static class MaybeGetters
{
    public static T Get<T>(this Maybe<T> maybe)
    {
        return maybe.Either(value => value, () => throw emptyException());
        InvalidOperationException emptyException() => new($"Cannot retrieve value. Maybe<{typeof(T).Name}> is empty");
    }
    public static T GetOrDefault<T>(this Maybe<T> maybe)
    {
        return maybe.Either(value => value, () => default!);
    }
    public static T GetOr<T>(this Maybe<T> maybe, T ifNone)
    {
        return maybe.Either(value => value, () => ifNone);
    }
    public static T GetOr<T>(this Maybe<T> maybe, Func<T> onNone)
    {
        return maybe.Either(value => value, onNone);
    }
    public static bool TryGetValue<T>(this Maybe<T> maybe, out T value)
    {
        value = maybe.GetOrDefault();
        return maybe.IsPresent;
    }
}
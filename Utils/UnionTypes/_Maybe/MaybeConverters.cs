namespace Utils.UnionTypes;

public static class MaybeConverters
{
    public static Attempt<T> ToResult<T>(this Maybe<T> maybe)
    {
        return maybe.Either(Results.Success, Results.Failure<T>);
    }
    public static T? ToReference<T>(this Maybe<T> maybe) where T : class
    {
        return maybe.IsPresent ? maybe.Get() : null;
    }
    public static T? ToNullable<T>(this Maybe<T> maybe) where T : struct
    {
        return maybe.IsPresent ? maybe.Get() : null;
    }
}
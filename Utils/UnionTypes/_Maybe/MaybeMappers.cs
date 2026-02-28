namespace Utils.UnionTypes._Maybe;

public static class MaybeMappers
{
    public static Maybe<U> Map<T, U>(this Maybe<T> maybe, Func<T, U> mapper)
    {
        return maybe.Either(value => Maybe<U>.Some(mapper(value)), Maybe<U>.None);
    }
    public static Maybe<U> FlatMap<T, U>(this Maybe<T> maybe, Func<T, Maybe<U>> mapper)
    {
        return maybe.Either(mapper, Maybe<U>.None);
    }
}
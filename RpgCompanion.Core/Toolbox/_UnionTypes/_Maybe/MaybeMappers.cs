namespace RpgCompanion.Core.Toolbox;

public static class MaybeMappers
{
    extension<T>(Maybe<T> maybe)
    {
        public Maybe<U> Map<U>(Func<T, U> mapper)
        {
            return maybe.Either(value => Maybe<U>.Some(mapper(value)), Maybe<U>.None);
        }

        public Maybe<U> FlatMap<U>(Func<T, Maybe<U>> mapper)
        {
            return maybe.Either(mapper, Maybe<U>.None);
        }
    }
}

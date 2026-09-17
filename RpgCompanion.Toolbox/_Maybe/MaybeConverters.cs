namespace RpgCompanion.Toolbox;

public static class MaybeConverters
{
    extension<T>(Maybe<T> maybe)
    {
        public Attempt<T> ToAttempt() => maybe.Either(Attempt<T>.Success, Results.Failure<T>);
    }
    extension<T>(Maybe<T> maybe) where T : class
    {
        public T? ToReference() => maybe.IsPresent ? maybe.Get() : null;
    }
    extension<T>(Maybe<T> maybe) where T : struct
    {
        public T? ToNullable()
        {
            return maybe.IsPresent ? maybe.Get() : null;
        }
    }
}

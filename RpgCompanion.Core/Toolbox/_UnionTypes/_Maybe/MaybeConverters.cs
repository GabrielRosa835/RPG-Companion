namespace RpgCompanion.Core.Toolbox;

public static class MaybeConverters
{
    extension<T>(Maybe<T> maybe)
    {
        public Attempt<T> ToResult()
        {
            return maybe.Either(Results.Success, Results.Failure<T>);
        }
    }
    extension<T>(Maybe<T> maybe) where T : class
    {
        public T? ToReference()
        {
            return maybe.IsPresent ? maybe.Get() : null;
        }
    }
    extension<T>(Maybe<T> maybe) where T : struct
    {
        public T? ToNullable()
        {
            return maybe.IsPresent ? maybe.Get() : null;
        }
    }
}

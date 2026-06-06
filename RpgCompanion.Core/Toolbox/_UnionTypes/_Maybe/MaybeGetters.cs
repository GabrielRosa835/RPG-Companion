namespace RpgCompanion.Core.Toolbox;

public static class MaybeGetters
{
    extension<T>(Maybe<T> maybe)
    {
        public T Get()
        {
            return maybe.Either(value => value, () => throw emptyException());
            InvalidOperationException emptyException() => new($"Cannot retrieve value. Maybe<{typeof(T).Name}> is empty");
        }

        public T GetOrDefault()
        {
            return maybe.Either(value => value, () => default!);
        }

        public T GetOr(T ifNone)
        {
            return maybe.Either(value => value, () => ifNone);
        }

        public T GetOr(Func<T> onNone)
        {
            return maybe.Either(value => value, onNone);
        }

        public bool TryGetValue(out T value)
        {
            value = maybe.GetOrDefault();
            return maybe.IsPresent;
        }
    }
}

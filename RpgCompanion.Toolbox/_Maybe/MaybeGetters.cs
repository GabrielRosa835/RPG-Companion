namespace RpgCompanion.Toolbox;

public static class MaybeGetters
{
    extension<T>(Maybe<T> maybe)
    {
        public T Get() => maybe.Either(value => value, () => throw Maybe<T>.EmptyException);
        public T GetOrDefault() => maybe.Either(value => value, () => default!);
        public T GetOr(T ifNone) => maybe.Either(value => value, () => ifNone);
        public T GetOr(Func<T> onNone) => maybe.Either(value => value, onNone);
        public bool IsPresent(out T value)
        {
            value = maybe.GetOrDefault();
            return maybe.IsPresent;
        }
        private static InvalidOperationException EmptyException => new($"Cannot retrieve value. Maybe<{typeof(T).Name}> is empty");
    }
}

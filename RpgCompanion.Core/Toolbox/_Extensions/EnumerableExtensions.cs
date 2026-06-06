namespace RpgCompanion.Core.Toolbox;

public static class EnumerableExtensions
{
    extension<T>(IEnumerable<T> enumerable)
    {
        public IEnumerable<U> ConvertAll<U>(Func<T, U> mapper)
        {
            if (enumerable is List<T> list)
            {
                return list.ConvertAll(mapper);
            }
            return enumerable.Select(mapper);
        }

        public void ForEach(Action<T> action)
        {
            if (enumerable is List<T> list)
            {
                list.ForEach(action);
            }
            foreach (var value in enumerable)
            {
                action(value);
            }
        }

        public Maybe<T> FirstOrEmpty()
        {
            return Results.Perhaps(enumerable.FirstOrDefault());
        }

        public Maybe<T> FirstOrEmpty(Func<T, bool> predicate)
        {
            return Results.Perhaps(enumerable.FirstOrDefault(predicate));
        }

        public IEnumerable<T> Peek(Action<T> action)
        {
            foreach (var value in enumerable)
            {
                action(value);
                yield return value;
            }
        }

        public string ToDisplayString(bool inNewLine = false)
        {
            return "[" + string.Join(inNewLine ? "\n" : ", ", enumerable) + "]";
        }

        public ICollection<T> AsCollection()
        {
            if (enumerable is ICollection<T> collection)
            {
                return collection;
            }
            return new List<T>(enumerable);
        }
    }
}

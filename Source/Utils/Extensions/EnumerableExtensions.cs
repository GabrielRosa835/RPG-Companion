using Utils.UnionTypes;

namespace Utils.Extensions;

public static class EnumerableExtensions
{
    public static Maybe<T> FirstOrDefaultAlt<T>(this IEnumerable<T> source)
    {
        return Results.Perhaps(source.FirstOrDefault());
    }
    public static Maybe<T> FirstOrDefaultAlt<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        return Results.Perhaps(source.FirstOrDefault(predicate));
    }
    public static T GetValueOrDefault<TKey, T>(this IDictionary<TKey, T> values, TKey key)
    {
        return values.TryGetValue(key, out var value) ? value : default!;
    }
    public static IEnumerable<T> Peek<T>(this IEnumerable<T> values, Action<T> action)
    {
        foreach (var value in values)
        {
            action(value);
            yield return value;
        }
    }
    public static string ToCompleteString<T>(this IEnumerable<T> values, bool inNewLine = false)
    {
        return "List[" + string.Join(inNewLine ? ", \n" : ", ", values) + "]";
    }
    public static void Set<T>(this ICollection<T> collection, IEnumerable<T> values)
    {
        collection.Clear();
        collection.AddRange(values);
    }
    public static ICollection<T> AsCollection<T>(this IEnumerable<T> values)
    {
        if (values is ICollection<T> collection)
        {
            return collection;
        }
        return new List<T>(values);
    }
}
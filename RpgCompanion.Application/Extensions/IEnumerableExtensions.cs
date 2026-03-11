namespace RpgCompanion.Application.Extensions;

public static class IEnumerableExtensions
{
    public static void Set<T>(this ICollection<T> collection, IEnumerable<T> items)
    {
        collection.Clear();
        if (collection is List<T> list)
        {
            list.AddRange(items);
            return;
        }
        foreach (T item in items)
        {
            collection.Add(item);
        }
    }
}
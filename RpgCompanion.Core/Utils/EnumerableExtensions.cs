namespace RpgCompanion.Core.Utils;

public static class EnumerableExtensions
{
   public static IEnumerable<T> SetValues<T>(this IEnumerable<T> enumerable, IEnumerable<T> values)
   {
      if (enumerable is List<T> list)
      {
         list.Clear();
         list.AddRange(values);
         return list;
      }
      if (enumerable is ICollection<T> collection)
      {
         collection.Clear();
         foreach (var item in values)
         {
            collection.Add(item);
         }
         return collection;
      }
      return values;
   }
}

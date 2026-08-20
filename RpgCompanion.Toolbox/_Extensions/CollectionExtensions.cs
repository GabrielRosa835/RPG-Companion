namespace RpgCompanion.Toolbox;

public static class CollectionExtensions
{
    extension<T>(ICollection<T> collection)
    {
        public void Set(IEnumerable<T> values)
        {
            collection.Clear();
            collection.AddRange(values);
        }

        public void AddRange(IEnumerable<T> values)
        {
            if (collection is List<T> list)
            {
                list.AddRange(values);
                return;
            }
            foreach (var value in values)
            {
                collection.Add(value);
            }
        }
    }
}

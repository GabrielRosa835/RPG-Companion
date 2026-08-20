namespace RpgCompanion.Toolbox;

public static class DictionaryExtensions
{
    extension<TKey, T>(IDictionary<TKey, T> values)
    {
        public T GetValueOrDefault(TKey key)
        {
            return values.TryGetValue(key, out var value) ? value : default!;
        }
        public Maybe<T> GetValueOrEmpty(TKey key)
        {
            return Results.Perhaps(values.TryGetValue(key, out var value) ? value : default!);
        }
    }
    extension<TKey, T>(KeyValuePair<TKey, T> pair)
    {
        public void Deconstruct(out TKey key, out T value)
        {
            key = pair.Key;
            value = pair.Value;
        }
    }
}

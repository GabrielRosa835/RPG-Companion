namespace RpgCompanion.Core.Engine;

using RpgCompanion.Core.Contexts;

internal class ContextData : IEditableContextData, IContextData
{
    private readonly Dictionary<string, dynamic> _data = [];

    internal bool Contains(ContextKey key) => _data.ContainsKey(key.Name);

    public T Get<T>(ContextKey<T> key) => (T)_data[key.Name];
    public void Set<T>(ContextKey<T> key, T value) => _data[key.Name] = value!;
    public bool Contains<T>(ContextKey<T> key) => _data.ContainsKey(key.Name);
    public bool TryGet<T>(ContextKey<T> key, out T value)
    {
        if (_data.TryGetValue(key.Name, out dynamic? v))
        {
            value = v;
            return true;
        }
        value = default!;
        return false;
    }
}

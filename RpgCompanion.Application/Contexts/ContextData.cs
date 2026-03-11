using RpgCompanion.Core.Contexts;

namespace RpgCompanion.Core.Engine;

internal class ContextData : IContextData, ISnapshotData
{
    private readonly Dictionary<string, dynamic> _data = new();
    
    internal bool Contains(ContextKey key) => _data.ContainsKey(key.Name);
    
    public T Get<T> (ContextKey<T> key) => (T) _data[key.Name];
    public void Set<T> (ContextKey<T> key, T value) => _data[key.Name] = value!;
    public bool Contains<T> (ContextKey<T> key) => _data.ContainsKey(key.Name);
    public bool TryGet<T> (ContextKey<T> key, out T value)
    {
        if (_data.TryGetValue(key.Name, out var v))
        {
            value = v;
            return true;
        }      
        value = default!;
        return false;
    }
}
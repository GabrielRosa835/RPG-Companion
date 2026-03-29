using RpgCompanion.Core.Engine.Contexts;

namespace RpgCompanion.Core.Engine;

internal class ContextData : IContextData
{
    private readonly Dictionary<string, dynamic> _data = [];

    public T Get<T>(ContextKey<T> key) => (T) _data[key.Name];
    public bool Contains<T>(ContextKey<T> key) => _data.ContainsKey(key.Name);
    public bool TryGet<T>(ContextKey<T> key, out T value)
    {
        if (_data.TryGetValue(key.Name, out var v))
        {
            value = v;
            return true;
        }
        value = default!;
        return false;
    }

    internal ContextData Set<T>(ContextKey<T> key, T value)
    {
        _data[key.Name] = value!;
        return this;
    }
}

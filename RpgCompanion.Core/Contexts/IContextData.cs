namespace RpgCompanion.Core.Contexts;

public interface IContextData
{
    bool Contains<T> (ContextKey<T> key);
    T Get<T> (ContextKey<T> key);
    void Set<T> (ContextKey<T> key, T value);
    bool TryGet<T> (ContextKey<T> key, out T value);
}
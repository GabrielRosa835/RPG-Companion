namespace RpgCompanion.Core.Contexts;

public interface IReadonlyContextData
{
   bool Contains<T> (ContextKey<T> key);
   T Get<T> (ContextKey<T> key);
   bool TryGet<T> (ContextKey<T> key, out T value);
}
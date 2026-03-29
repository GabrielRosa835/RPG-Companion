namespace RpgCompanion.Core.Engine.Contexts;

// Singleton (Metadata collection)
public interface IContextData
{
   bool Contains<T> (ContextKey<T> key);
   T Get<T> (ContextKey<T> key);
   bool TryGet<T> (ContextKey<T> key, out T value);
}

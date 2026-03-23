namespace RpgCompanion.Core.Contexts;

public interface IEditableContextData : IContextData
{
    void Set<T> (ContextKey<T> key, T value);
}

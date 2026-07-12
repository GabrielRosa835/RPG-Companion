namespace RpgCompanion.Core;

public abstract record Rel<T> where T : class, IEntity<T>
{
    public sealed record Loaded(T Entity) : Rel<T>;
    public sealed record Unloaded(DatabaseId<T> DbId) : Rel<T>;
}

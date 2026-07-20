namespace RpgCompanion.Core;

public static class Rel
{
    public static Rel<T>.None None<T>() where T : class, IEntity => new();
    public static Rel<T>.Loaded Loaded<T>(T entity) where T : class, IEntity => new(entity);
    public static Rel<T>.Unloaded Unloaded<T>(DatabaseId<T> dbId) where T : class, IEntity => new(dbId);
}

public abstract record Rel<T> where T : class, IEntity
{
    public sealed record None : Rel<T>;

    public sealed record Loaded(T Entity) : Rel<T>
    {
        public static implicit operator Loaded(T entity) => new(entity);
    }

    public sealed record Unloaded(DatabaseId<T> DbId) : Rel<T>
    {
        public static implicit operator Unloaded(DatabaseId<T> dbId) => new(dbId);
    }
}

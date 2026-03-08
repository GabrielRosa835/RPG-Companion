namespace RpgCompanion.Core.Contexts;

public abstract record ContextKey (string Name);

public record ContextKey<T> (string Name) : ContextKey(Name);
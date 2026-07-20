namespace RpgCompanion.Core;

public readonly record struct DatabaseId
{
    public string Value { get; }
    public DatabaseId() => Value = Guid.CreateVersion7().ToString("N");
    public DatabaseId(string value) => Value = value;

    public DatabaseId<TFor> For<TFor>() where TFor : class, IEntity<TFor> => new(Value);
}

public readonly record struct DatabaseId<TFor> where TFor : class, IEntity
{
    public string Value { get; }
    public DatabaseId() => Value = Guid.CreateVersion7().ToString("N");
    public DatabaseId(string value) => Value = value;

    public static implicit operator DatabaseId(DatabaseId<TFor> typed) => new(typed.Value);
    public static implicit operator DatabaseId<TFor>(DatabaseId untyped) => new(untyped.Value);
}

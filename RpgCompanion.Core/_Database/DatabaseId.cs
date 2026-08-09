namespace RpgCompanion.Core;

public record DatabaseId
{
    public string Value { get; internal init; }
    internal DatabaseId(string value) => Value = value;
    internal DatabaseId() : this(Guid.CreateVersion7().ToString("N"))
    {
    }

    public DatabaseId<TFor> For<TFor>() where TFor : IEntity => new(Value);

    public static DatabaseId Create() => new();
    public static DatabaseId Create(string value) => new(value);
    public static DatabaseId<TFor> Create<TFor>() where TFor : IEntity => new();
    public static DatabaseId<TFor> Create<TFor>(string value) where TFor : IEntity => new(value);
}

public record DatabaseId<TFor> : DatabaseId where TFor : IEntity
{
    internal DatabaseId(string value) : base(value) { }
    internal DatabaseId() : base(Guid.CreateVersion7().ToString("N"))
    {
    }
}

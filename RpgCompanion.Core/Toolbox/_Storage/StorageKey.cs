namespace RpgCompanion.Core.Toolbox;

public readonly record struct StorageKey(string Value)
{
    public static implicit operator StorageKey(string value) => new(value);
    public static implicit operator string(StorageKey key) => key.Value;

    public static StorageKey Of(string value) => new(value);
    public static StorageKey<T> Of<T>(string value) => new(value);

    public StorageKey<T> For<T>() => new(Value);
}

public readonly record struct StorageKey<T>(string Value)
{
    public static implicit operator StorageKey<T>(string value) => new(value);
    public static implicit operator string(StorageKey<T> key) => key.Value;

    public static implicit operator StorageKey<T>(StorageKey key) => new(key.Value);
}

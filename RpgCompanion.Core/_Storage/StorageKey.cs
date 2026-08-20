namespace RpgCompanion.Core;

public readonly record struct StorageKey<T>(string Value)
{
    public static implicit operator StorageKey<T>(string value) => new(value);
    public static implicit operator string(StorageKey<T> key) => key.Value;
}

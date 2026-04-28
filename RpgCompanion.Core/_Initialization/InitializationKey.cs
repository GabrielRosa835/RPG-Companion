namespace RpgCompanion.Core;

public readonly record struct InitializationKey(string Value)
{
    public InitializationKey() : this(Guid.NewGuid().ToString()) { }
}

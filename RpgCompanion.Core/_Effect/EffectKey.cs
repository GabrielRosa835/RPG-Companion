namespace RpgCompanion.Core;

public record struct EffectKey(string Value)
{
    public EffectKey() : this(Guid.NewGuid().ToString()) { }
}

namespace RpgCompanion.Core;

public readonly record struct RuleKey(string Value)
{
    public RuleKey() : this(Guid.NewGuid().ToString()) { }
}

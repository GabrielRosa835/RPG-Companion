namespace RpgCompanion.Core;

public readonly record struct ConditionKey(string Value)
{
    public ConditionKey() : this(Guid.NewGuid().ToString())
    {
    }
}

namespace RpgCompanion.Core;

public record struct EventKey(string Value)
{
    public EventKey() : this(Guid.NewGuid().ToString()) { }
}

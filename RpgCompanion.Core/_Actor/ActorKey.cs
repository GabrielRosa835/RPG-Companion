namespace RpgCompanion.Core;

public readonly record struct ActorKey(string Value)
{
    public ActorKey() :  this(Guid.NewGuid().ToString()) { }
}

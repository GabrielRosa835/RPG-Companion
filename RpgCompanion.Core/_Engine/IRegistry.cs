namespace RpgCompanion.Core;

public interface IRegistry
{
    public TActor? GetOrDefault<TActor>() where TActor : class, IActor;
    public TActor Get<TActor>() where TActor : class, IActor;
}

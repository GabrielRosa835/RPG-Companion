namespace RpgCompanion.Core;

public interface IContext
{
    IPastEvents PastEvents { get; }
    IContextData Data { get; }
    IRegistry Registry { get; }
}

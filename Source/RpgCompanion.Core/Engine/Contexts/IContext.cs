namespace RpgCompanion.Core.Engine.Contexts;

using Application.Engine.Contexts;

public interface IContext
{
    IPastEvents PastEvents { get; }
    IContextData Data { get; }
    IRegistry Registry { get; }
}

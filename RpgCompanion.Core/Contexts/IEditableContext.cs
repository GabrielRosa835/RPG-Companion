namespace RpgCompanion.Core.Contexts;

using RpgCompanion.Core.Engine;

public interface IEditableContext
{
    public IEditableContextData Data { get; }
    public IRegistry Registry { get; }
}

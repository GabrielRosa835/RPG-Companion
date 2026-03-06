using RpgCompanion.Core.Events;

namespace RpgCompanion.Core.Engine;

public interface IContextProvider
{
   Context Bundle(IEvent @event);
}
using RpgCompanion.Core.Events;

namespace RpgCompanion.Core.Engine;

public interface IContextProvider
{
   IContext Bundle(IEvent @event);
}
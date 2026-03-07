using RpgCompanion.Core.Events;

namespace RpgCompanion.Core.Engine;

public interface IContext
{
   IContextData Data { get; }
   IRegistry Registry { get; }


   void Raise (IEvent @event);
}
using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;

namespace RpgCompanion.Core.Contexts;

public interface IContext
{
   IContextData Data { get; }
   IRegistry Registry { get; }

   void Raise (IEvent @event);
}
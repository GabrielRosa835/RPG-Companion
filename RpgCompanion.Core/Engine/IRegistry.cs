using RpgCompanion.Core.Events;
using RpgCompanion.Core.Events.Producers;

namespace RpgCompanion.Core.Engine;

public interface IRegistry
{
   T? Get<T> ();
   T GetRequired<T> ();

   IEnumerable<IInterceptor> GetInterceptorsFor (IEvent @event);
   IEnumerable<IEventHandler> GetHandlersFor (IEvent @event);
}
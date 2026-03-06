using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Utils;

namespace RpgCompanion.Core.Events;

public interface IEventHandler
{
   void Handle (IEvent @event, Context context);
}
public interface IEventHandler<in TEvent> : IEventHandler where TEvent : IEvent<IEventProducer>
{
   void Handle (TEvent @event, Context context);
   void IEventHandler.Handle (IEvent @event, Context context) => Handle (@event.As<TEvent>(), context);
}
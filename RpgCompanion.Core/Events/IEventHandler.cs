using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Utils;

namespace RpgCompanion.Core.Events;

public interface IEventHandler
{
   void Handle (IEvent @event, IContext context);
}
public interface IEventHandler<in TEvent> : IEventHandler where TEvent : IEvent<IEventProducer>
{
   void Handle (TEvent @event, IContext context);
   void IEventHandler.Handle (IEvent @event, IContext context) => Handle (@event.As<TEvent>(), context);
}
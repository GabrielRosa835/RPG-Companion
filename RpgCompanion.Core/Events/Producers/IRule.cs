using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;

namespace RpgCompanion.Core.Events.Producers;

public interface IRule<TEvent> : IEventProducer where TEvent : IEvent<IRule<TEvent>>
{
   TEvent Handle (IContext context);
}

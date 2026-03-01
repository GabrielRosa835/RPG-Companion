using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Objects;

namespace RpgCompanion.Core.Events.Producers;

public interface IEffect<TEvent> : IEventProducer where TEvent : IEvent<IEffect<TEvent>>
{
   TEvent Apply (IObject @object, IContext context);
}

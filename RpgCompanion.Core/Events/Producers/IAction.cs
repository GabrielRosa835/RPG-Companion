using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Objects;

namespace RpgCompanion.Core.Events.Producers;

public interface IAction<out TEvent> : IEventProducer where TEvent : IEvent<IAction<TEvent>>
{
   TEvent For (IActor actor, IContext context);
}
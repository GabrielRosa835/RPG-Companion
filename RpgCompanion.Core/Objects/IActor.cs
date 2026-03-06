using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Events.Producers;

namespace RpgCompanion.Core.Objects;

public interface IActor : IObject
{
   void Act<TEvent>(IAction<TEvent> action, Context context) where TEvent : IEvent<IAction<TEvent>>;
}

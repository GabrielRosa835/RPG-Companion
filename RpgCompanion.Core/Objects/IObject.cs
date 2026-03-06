using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Events.Producers;

namespace RpgCompanion.Core.Objects;

public interface IObject
{
   void Accept<TEvent> (IEffect<TEvent> effect, Context context) where TEvent : IEvent<IEffect<TEvent>>;
}

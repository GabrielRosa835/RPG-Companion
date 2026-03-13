using RpgCompanion.Core.Contexts;

namespace RpgCompanion.Core.Events;

public interface IEffect<in TEvent> where TEvent : IEvent
{
   bool ShouldApply(TEvent @event, IContext context);
   void Apply (TEvent @event, IContext context);
}
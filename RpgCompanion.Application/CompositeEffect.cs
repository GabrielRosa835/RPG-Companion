using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;

namespace RpgCompanion.Application;

internal record CompositeEvent (IEnumerable<IEvent> Events) : IEvent
{
   public string Name => nameof(CompositeEvent);
}

internal class CompositeEffect : IEffect<CompositeEvent>
{
   public void Apply (CompositeEvent @event, IContext context)
   {
      foreach(var inner in @event.Events)
      {
         context.Raise(inner);
      }
   }
}

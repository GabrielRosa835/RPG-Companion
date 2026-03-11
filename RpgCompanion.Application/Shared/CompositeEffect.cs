using RpgCompanion.Core.Contexts;
using RpgCompanion.Core.Events;

namespace RpgCompanion.Application;

internal record CompositeEvent (IEnumerable<IEvent> Events) : IEvent
{
   public string Name => nameof(CompositeEvent);
   public int Priority => int.MaxValue;
}

internal class CompositeEffect : IEffect<CompositeEvent>
{
   public bool ShouldApply(CompositeEvent @event, IContext context)
   {
      return true;
   }
   public void Apply (CompositeEvent @event, IContext context)
   {
      foreach(var inner in @event.Events)
      {
         context.Raise(inner);
      }
   }
}

using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;

namespace RpgCompanion.Application;

internal record CompositeEvent (IEnumerable<IEvent> Events) : IEvent<IEventProducer>;

internal class CompositeEventHandler : IEventHandler<CompositeEvent>
{
   public void Handle (CompositeEvent @event, Context context)
   {
      foreach(var inner in @event.Events)
      {
         context.Engine.EventQueue.Raise (inner);
      }
   }
}

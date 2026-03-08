using RpgCompanion.Core.Events;

using System.Collections;

namespace RpgCompanion.Application;

internal class EventQueue : IEnumerable<IEvent>
{
   private readonly Queue<IEvent> _queue;

   public EventQueue (Queue<IEvent> queue)
   {
      _queue = queue;
   }

   public IEvent Dequeue ()
   {
      return _queue.Dequeue();
   }
   public void Enqueue(IEvent @event)
   {
      _queue.Enqueue(@event);
   }

   public IEnumerator<IEvent> GetEnumerator () => ((IEnumerable<IEvent>) _queue).GetEnumerator();
   IEnumerator IEnumerable.GetEnumerator () => ((IEnumerable) _queue).GetEnumerator();
}

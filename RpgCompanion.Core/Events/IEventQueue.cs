namespace RpgCompanion.Core.Events;

public interface IEventQueue
{
   bool Any ();
   void Push (IEvent @event);
   IEvent Pop ();
}

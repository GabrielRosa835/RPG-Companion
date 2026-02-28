namespace RpgCompanion.Canva;

public interface IEventQueue
{
   bool Any ();
   void Push (IEvent @event);
   IEvent Pop ();
}

namespace RpgCompanion.Core.Events;

public interface IEvent
{
   string Name { get; }

   public static IEvent Empty => new EmptyEvent();
}
public interface IEvent<out T> : IEvent where T : IEventProducer
{
   public static new IEvent<T> Empty => new EmptyEvent<T>();
}
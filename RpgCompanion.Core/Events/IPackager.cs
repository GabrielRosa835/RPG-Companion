using RpgCompanion.Core.Events;

namespace RpgCompanion.Core.Contexts;

public interface IPackager<in TEvent> where TEvent : IEvent
{
    void Pack (TEvent @event, IContext context);
}

public record EmptyPackager<TEvent> : IPackager<TEvent> where TEvent : IEvent
{
    public void Pack(TEvent @event, IContext context)
    {
        Console.WriteLine(@event.GetType().Name);
    }
}
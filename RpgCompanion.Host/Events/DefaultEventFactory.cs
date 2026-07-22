namespace RpgCompanion.Host.Events;

public class DefaultEventFactory(IServiceProvider _serviceProvider) : IEventFactory
{
    public TEvent Create<TEvent>() where TEvent : Event
    {
        return _serviceProvider.GetService<TEvent>()!;
    }

    public Event Create(Type eventType)
    {
        return (Event) _serviceProvider.GetService(eventType)!;
    }
}

namespace RpgCompanion.Host;

using System.Reflection;
using System.Text.Json;
using Core;
using MassTransit;

internal class EventRaisedRouter(
    IServiceProvider _serviceProvider,
    IComponentGraph _components)
    : IConsumer<EventRaisedEvent>
{
    private const string Handle = nameof(EventRaisedHandler<>.Handle);
    private static Type HandlerType => typeof(EventRaisedHandler<>);

    public Task Consume(ConsumeContext<EventRaisedEvent> context)
    {
        var eventDescriptor = _components.Events.FirstOrDefault(e => e.Key == context.Message.Key);
        if (eventDescriptor is null)
        {
            throw new Exception("Problem");
        }

        var concreteEvent = context.Message.Data.Deserialize(eventDescriptor.Type);
        if (concreteEvent is null)
        {
            throw new Exception("Problem");
        }

        var handlerType = HandlerType.MakeGenericType(eventDescriptor.Type);
        var handler = ActivatorUtilities.CreateInstance(_serviceProvider, handlerType);
        object[] args = [concreteEvent, context.Message.Transitions, context, context.CancellationToken];

        return (Task) handlerType.GetMethod(Handle)!.Invoke(handler, args)!;
    }
}

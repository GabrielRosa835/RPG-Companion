namespace RpgCompanion.Host;

using MediatR;

internal class EventRouter(IServiceProvider _serviceProvider) : INotificationHandler<EventContext>
{
    public Task Handle(EventContext context, CancellationToken cancellationToken)
    {
        var handler = EventHandler.CreateTyped(context.Descriptor.Type, _serviceProvider);
        handler.Handle(context);
        return Task.CompletedTask;
    }
}

namespace RpgCompanion.Host;

using MediatR;

internal class EventPublisher(IMediator _mediator) : IEventPublisher
{
    public void Publish(EventContext context)
    {
        _mediator.Publish(context).GetAwaiter().GetResult();
    }
}

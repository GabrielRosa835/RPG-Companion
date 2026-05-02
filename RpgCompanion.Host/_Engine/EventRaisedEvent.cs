namespace RpgCompanion.Host;

using Core;
using global::MediatR;

public record EventRaisedEvent : INotification
{
    public IEvent Event { get; init; } = default!;
    public EventDescriptor Descriptor { get; init; } = default!;
    public Queue<Func<IEvent, IEvent>> Transitions { get; init; } = [];
}

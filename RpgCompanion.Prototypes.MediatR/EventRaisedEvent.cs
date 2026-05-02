namespace RpgCompanion.Prototypes.MediatR;

using Core;
using global::MediatR;

public record EventRaisedEvent<TEvent> : INotification where TEvent : IEvent
{
    public TEvent Event { get; init; } = default!;
    public EventKey DescriptorKey { get; init; } = default!;
    public Queue<RuleKey> Transitions { get; init; } = [];
}

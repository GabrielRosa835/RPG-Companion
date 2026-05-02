namespace RpgCompanion.Prototypes.MassTransit;

using Core;

public record EventRaisedEvent<TEvent>
{
    public TEvent Event { get; init; } = default!;
    public EventKey DescriptorKey { get; init; } = default!;
    public Queue<RuleKey> Transitions { get; init; } = [];
}

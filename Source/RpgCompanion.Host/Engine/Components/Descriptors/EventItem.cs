namespace RpgCompanion.Application.Services;

using Core.Events;

internal class EventItem
{
    public Guid Id { get; } = Guid.NewGuid();
    public required EventDescriptor Descriptor { get; init; } = default!;
    public IEvent? Instance { get; set; }
    public EventContinuation? Continuation { get; set; }
    public int Priority => this.Descriptor.EffectivePriority;
}

internal record EventContinuation(Delegate Sequence, EventItem Item);

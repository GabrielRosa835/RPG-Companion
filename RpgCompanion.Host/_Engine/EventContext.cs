using RpgCompanion.Core;

namespace RpgCompanion.Host;

using MediatR;

internal record EventContext : INotification
{
    public Guid Id { get; } = Guid.CreateVersion7();
    public IEvent Data { get; init; } = default!;
    public EventDescriptor Descriptor { get; init; } = default!;
    public List<EventTransition> Transitions { get; init; } = [];
}

internal record EventTransition
{
    public Rule<IEvent, IEvent> Rule { get; init; } = default!;
    public List<EventTransition> Chain { get; init; } = [];
}

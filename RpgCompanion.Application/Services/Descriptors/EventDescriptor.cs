namespace RpgCompanion.Application.Services;

using Core.Events;

internal class EventDescriptor
{
    public Type Type { get; set; } = default!;
    public string? DisplayName { get; set; }
    public int Priority { get; set; }
    public int EffectivePriority => int.MaxValue - this.Priority;

    public EventItem CreateEvent(IEvent obj) => new()
    {
        Descriptor = this,
        Instance = obj,
    };

    public EventItem CreateEvent() => new()
    {
        Descriptor = this,
    };
}

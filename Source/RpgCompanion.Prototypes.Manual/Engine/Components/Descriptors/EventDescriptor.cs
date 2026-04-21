namespace RpgCompanion.Application.Services;

using Core.Events;

internal class EventDescriptor
{
    public Type Type { get; set; } = default!;
    public string? DisplayName { get; set; }
    public int Priority { get; set; }
    public int EffectivePriority => int.MaxValue - this.Priority;

    public EventItem CreateItem(IEvent obj) => new()
    {
        Descriptor = this,
        Instance = obj,
    };

    public EventItem CreateItem() => new()
    {
        Descriptor = this,
    };
}

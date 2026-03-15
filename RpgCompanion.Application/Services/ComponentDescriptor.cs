using RpgCompanion.Core.Events;

namespace RpgCompanion.Application.Services;

internal abstract class ComponentDescriptor
{
   public object Instance { get; set; } = default!;
   public Type ComponentType { get; init; } = default!;
   public Type GenericType { get; init; } = default!;
}

internal class CustomDescriptor : ComponentDescriptor
{

}

internal class EventDescriptor : ComponentDescriptor
{
   public string? DisplayName { get; internal set; }
   public int Priority { get; internal set; }
   public int EffectivePriority => int.MaxValue - Priority;
   public new IEvent Instance { get => (IEvent)base.Instance; internal set => base.Instance = value; }
   public List<Delegate> Continuations { get; } = [];
}

internal class RuleDescriptor : ComponentDescriptor
{
   public Type EventType { get; init; } = default!;
   public RuleOrdering Ordering { get; init; }
}

internal class EffectDescriptor : ComponentDescriptor
{
   public Type EventType { get; init; } = default!;
}

internal class PackagerDescriptor : ComponentDescriptor
{
   public Type EventType { get; init; } = default!;
}

namespace RpgCompanion.Application.Services;

internal abstract class ComponentDescriptor
{
   public object Instance { get; set; } = default!;
   public Type ComponentType { get; set; } = default!;
   public Type GenericType { get; set; } = default!;
}

internal class CustomDescriptor : ComponentDescriptor
{

}

internal class EventDescriptor : ComponentDescriptor
{
   public string? DisplayName { get; set; }
   public int Priority { get; set; }
}

internal class RuleDescriptor : ComponentDescriptor
{
   public Type EventType { get; set; } = default!;
}

internal class EffectDescriptor : ComponentDescriptor
{
   public Type EventType { get; set; } = default!;
}

internal class ContractDescriptor : ComponentDescriptor
{
   public Type EventType { get; set; } = default!;
}

internal class PackagerDescriptor : ComponentDescriptor
{
   public Type EventType { get; set; } = default!;
}
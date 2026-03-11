using RpgCompanion.Core.Meta;

namespace RpgCompanion.Application.Services;

internal class ComponentDescriptor
{
   public object Instance { get; set; } = default!;
   public Type ComponentType { get; set; } = default!;
   public Type? EventType { get; set; }
   public Type GenericType { get; set; } = default!;
   public ComponentCategory Category { get; set; }
}

public enum ComponentCategory
{
   Custom,
   Rule,
   Effect,
   Contract,
   Packager,
}
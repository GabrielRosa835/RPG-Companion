using RpgCompanion.Core.Meta;

namespace RpgCompanion.Application.Services;

internal class ComponentDescriptor
{
   public object? ComponentInstance { get; set; } = default!;
   public Type ComponentType { get; set; } = default!;
   public Type? EventType { get; set; }
   public IPlugin Plugin { get; set; } = default!;
   public ComponentCategory Category { get; set; } = default!;

   public RulePlacement? Rule_Placement { get; set; }
   public int? Effect_Priority { get; set; }
}

public enum EffectPriority
{
   None = 0,
   Low = 100,
   Medium = 200,
   High = 300,
}

public enum RulePlacement
{
   None,
   BeforeEvent,
   AfterEvent,
   BeforeEffect,
   AfterEffect,
}

public enum ComponentCategory
{
   Custom,
   Rule,
   Effect,
   Contract,
   Template,
   EffectChecker,
   RuleChecker,
}

internal static class EffectPriorityExtensions
{
   public static int Value(this EffectPriority priority) => (int) priority;
}
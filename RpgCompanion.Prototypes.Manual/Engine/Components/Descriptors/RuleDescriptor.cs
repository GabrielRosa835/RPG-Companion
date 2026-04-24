using RpgCompanion.Application.Reflection;
using RpgCompanion.Core.Engine.Contexts;
using RpgCompanion.Core.Events;

namespace RpgCompanion.Application.Services;

internal class RuleDescriptor : ComponentDescriptor
{
    public Type EventType { get; set; } = default!;
    public Delegate? Condition { get; set; }
    public Delegate? Action { get; set; }
    public RuleOrdering Ordering { get; set; }

    public bool ShouldApply(Reflect reflect, IContext context) => (bool)
        InvokeMethod(reflect, nameof(IRule<>.ShouldApply), this.Condition, context)!;

    public IEvent Apply(Reflect reflect, IContext context) => (IEvent)
        InvokeMethod(reflect, nameof(IRule<>.Apply), this.Action, context)!;
}

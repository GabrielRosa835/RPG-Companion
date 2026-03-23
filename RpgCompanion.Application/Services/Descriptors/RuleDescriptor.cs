namespace RpgCompanion.Application.Services;

using Core.Contexts;
using Core.Events;
using Core.Events.Producers;
using Reflection;

internal class RuleDescriptor : ComponentDescriptor
{
    public Type EventType { get; set; } = default!;
    public Delegate? Condition { get; set; }
    public Delegate? Action { get; set; }
    public RuleOrdering Ordering { get; set; }

    public bool ShouldApply(Reflect reflect, IContext context) =>
        InvokeMethod(reflect, nameof(IRule<>.ShouldApply), this.Condition, context).As<bool>();

    public IEvent Apply(Reflect reflect, IContext context) =>
        InvokeMethod(reflect, nameof(IRule<>.Apply), this.Action, context).As<IEvent>();
}

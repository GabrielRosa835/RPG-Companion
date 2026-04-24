namespace RpgCompanion.Application.Services;

using Core.Engine;
using Core.Events;
using Reflection;

internal class EffectDescriptor : ComponentDescriptor
{
    public Type EventType { get; set; } = default!;
    public Delegate? Condition { get; set; }
    public Delegate? Action { get; set; }

    public bool ShouldApply(Reflect reflect, IEvent e) => (bool)
        InvokeMethod(reflect, nameof(IEffect<>.ShouldApply), this.Condition, e)!;
    public void Apply(Reflect reflect, IEvent e, IPipeline pipeline) =>
        InvokeMethod(reflect, nameof(IEffect<>.Apply), this.Action, e, pipeline);
}

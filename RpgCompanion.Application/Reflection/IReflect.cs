namespace RpgCompanion.Application.Reflection;

using RpgCompanion.Application.Services;
using RpgCompanion.Core.Contexts;
using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;

internal interface IReflect
{
    public bool ShouldApply(RuleDescriptor descriptor, IContext snapshot);
    public IEvent Apply(RuleDescriptor descriptor, IContext snapshot);
}
internal interface IReflectEffect
{
    public bool ShouldApply(EffectDescriptor descriptor, IEvent e, IPipeline pipeline);
    public void Apply(EffectDescriptor descriptor, IEvent e, IPipeline pipeline);
}
internal interface IReflectPackager
{
    public void Pack(PackagerDescriptor descriptor, IEvent e, IEditableContext context);
}

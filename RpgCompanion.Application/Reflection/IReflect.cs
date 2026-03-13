using RpgCompanion.Application.Services;
using RpgCompanion.Core.Contexts;
using RpgCompanion.Core.Events;

namespace RpgCompanion.Application.Reflection;

internal interface IReflect
{
    bool ShouldApply(RuleDescriptor descriptor, ISnapshot snapshot);
    IEvent Apply(RuleDescriptor descriptor, ISnapshot snapshot);
}

internal interface IReflectEffect
{
    bool ShouldApply(EffectDescriptor descriptor, IEvent @event, IContext context);
    void Apply(EffectDescriptor descriptor, IEvent @event, IContext context);
}

internal interface IReflectContract
{
    IEnumerable<ContextKey> Requirements(ContractDescriptor descriptor);
    IEnumerable<ContextKey> Outputs(ContractDescriptor descriptor);
}

internal interface IReflectPackager
{
    void Pack(PackagerDescriptor descriptor, IEvent @event, IContext context);
}
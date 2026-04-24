using RpgCompanion.Application.Engines;
using RpgCompanion.Application.Reflection;
using RpgCompanion.Application.Services;
using RpgCompanion.Core.Events;

namespace RpgCompanion.Application;

// Scoped
internal class EventExecutor(
    ComponentProvider componentProvider, // Transient
    EventQueue queue, // Singleton
    Reflect reflect, // Singleton
    Context context, // Scoped
    Pipeline pipeline) // Transient
{
    private readonly List<IEvent> _eventCache = [];

    // TODO: Implementar formas assíncronas de efeitos e regras
    internal async Task Execute(EventItem currentEvent, CancellationToken cancellationToken = default)
    {
        var effect = componentProvider.GetEffectFor(currentEvent.Descriptor.Type);
        var rules = componentProvider.GetRulesFor(currentEvent.Descriptor.Type);

        _eventCache.AddRange(rules.Before()
            .Where(rule => rule.ShouldApply(reflect, context))
            .Select(rule => rule.Apply(reflect, context)));

        if (effect is not null && effect.ShouldApply(reflect, currentEvent.Instance!))
        {
            effect.Apply(reflect, currentEvent.Instance!, pipeline);
            context.InternalPastEvents.Enqueue(currentEvent.Instance!);
        }

        _eventCache.AddRange(rules.After()
            .Where(rule => rule.ShouldApply(reflect, context))
            .Select(rule => rule.Apply(reflect, context)));

        if (currentEvent.Continuation is not null)
        {
            var other = (IEvent) currentEvent.Continuation.Sequence.DynamicInvoke(currentEvent.Instance)!;
            currentEvent.Continuation.Item.Instance = other;
            queue.Enqueue(currentEvent.Continuation.Item);
        }

        if (_eventCache.Count > 0)
        {
            queue.EnqueueRange(_eventCache
                .GroupBy(e => e.GetType())
                .SelectMany(eGroup =>
                {
                    var descriptor = componentProvider.GetEventDescriptor(eGroup.Key);
                    return eGroup.Select(e => descriptor.CreateItem(e));
                }));
        }
    }
}

file static class Utils
{
    extension(IEnumerable<RuleDescriptor> rules)
    {
        public IEnumerable<RuleDescriptor> Before () => rules
            .Where(d => d is not { HasService: true, Instance: not null })
            .Where(d => d.Ordering.HasFlag(RuleOrdering.Before));

        public IEnumerable<RuleDescriptor> After () => rules
            .Where(d => d is not { HasService: true, Instance: not null })
            .Where(d => d.Ordering.HasFlag(RuleOrdering.After));
    }
}

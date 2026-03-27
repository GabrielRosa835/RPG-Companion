namespace RpgCompanion.Application;

using Core.Engine;
using Core.Events;
using Engines;
using Reflection;
using Services;

internal class Engine(Reflect reflect, EventQueue queue)
{
    private readonly RuleCollection _rules = new();
    private readonly List<IEvent> _eventCache = [];

    public async Task Execute(ComponentProvider componentProvider, EventItem current, CancellationToken cancellationToken = default)
    {
        var pipeline = new Pipeline(queue, componentProvider);
        var context = new Context(componentProvider);
        var bundlerDesc = componentProvider.GetBundlerDescriptorFor(current.Descriptor.Type);
        var effectDesc = componentProvider.GetEffectDescriptorFor(current.Descriptor.Type);
        _rules.SetValues(componentProvider.GetRulesDescriptorsFor(current.Descriptor.Type));

        if (bundlerDesc is not null)
        {
            bundlerDesc.Apply(reflect, current.Instance!, context);
        }

        _eventCache.AddRange(_rules.Before
            .Where(rule => rule.ShouldApply(reflect, context))
            .Select(rule => rule.Apply(reflect, context)));

        if (effectDesc is not null && effectDesc.ShouldApply(reflect, current.Instance!))
        {
            effectDesc.Apply(reflect, current.Instance!, pipeline);
        }

        _eventCache.AddRange(_rules.After
            .Where(rule => rule.ShouldApply(reflect, context))
            .Select(rule => rule.Apply(reflect, context)));

        if (current.Continuation is not null)
        {
            var continuation = current.Continuation.Value;
            var other = continuation.Sequence.DynamicInvoke(current.Instance).As<IEvent>();
            continuation.Item.Instance = other;
            queue.Enqueue(continuation.Item);
        }

        if (_eventCache.Count > 0)
        {
            queue.EnqueueRange(_eventCache
                .GroupBy(e => e.GetType())
                .SelectMany(eGroup =>
                {
                    var descriptor = componentProvider.GetEventDescriptor(eGroup.Key);
                    return eGroup.Select(e => descriptor.CreateEvent(e));
                }));
        }
    }
}

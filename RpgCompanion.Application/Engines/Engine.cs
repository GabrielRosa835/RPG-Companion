namespace RpgCompanion.Application;

using Core.Events;
using Engines;
using Reflection;

internal class Engine(Reflect reflect)
{
    private readonly RuleCollection _rules = new();
    private readonly List<IEvent> _eventCache = [];

    public async Task Execute(EventQueue queue, ComponentProvider componentProvider, CancellationToken cancellationToken = default)
    {
        var context = new Context(componentProvider);
        var pipeline = new Pipeline(componentProvider, queue);
        var eventDesc = queue.Dequeue();
        var templateDesc = componentProvider.GetBundlerDescriptorFor(eventDesc.Descriptor.Type);
        var effectDesc = componentProvider.GetEffectDescriptorFor(eventDesc.Descriptor.Type);
        _rules.SetValues(componentProvider.GetRulesDescriptorsFor(eventDesc.Descriptor.Type));

        if (templateDesc is not null)
        {
            templateDesc.Apply(reflect, eventDesc.Instance!, context);
        }

        _eventCache.AddRange(_rules.Before
            .Where(rule => rule.ShouldApply(reflect, context))
            .Select(rule => rule.Apply(reflect, context)));

        if (effectDesc is not null && effectDesc.ShouldApply(reflect, eventDesc.Instance!))
        {
            effectDesc.Apply(reflect, eventDesc.Instance!, pipeline);
        }

        _eventCache.AddRange(_rules.After
            .Where(rule => rule.ShouldApply(reflect, context))
            .Select(rule => rule.Apply(reflect, context)));

        if (eventDesc.Continuation is not null)
        {
            var other = eventDesc.Continuation.Value.Sequence.DynamicInvoke(eventDesc.Instance).As<IEvent>();
            eventDesc.Continuation.Value.Item.Instance = other;
            _eventCache.Add(other);
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

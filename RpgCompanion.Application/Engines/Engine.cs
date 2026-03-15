namespace RpgCompanion.Application;

using RpgCompanion.Application.Engines;
using RpgCompanion.Application.Reflection;
using RpgCompanion.Core.Events;

using Utils.UnionTypes;

internal class Engine
{
    private readonly PluginManager _pluginManager = default!;
    private readonly Reflect _reflect = default!;
    private readonly RuleCollection _rules = new();
    private readonly EventQueue _queue = new();

    internal EventQueue Queue => _queue;

    public async Task Execute(PluginDescriptor plugin)
    {
        while (!Queue.Any())
        {
            await Task.Delay(100);
        }

        // Preparation
        if (await _pluginManager.Load(plugin).IsFailure())
        {
            return;
        }
        var context = new Context(plugin);
        var pipeline = new Pipeline(plugin.Registry, _queue);
        var eventDesc = Queue.Dequeue();
        var templateDesc = plugin.Registry.GetPackagerDescriptorFor(eventDesc.ComponentType);
        var effectDesc = plugin.Registry.GetEffectDescriptorFor(eventDesc.ComponentType);
        _rules.SetValues(plugin.Registry.GetRulesDescriptorsFor(eventDesc.ComponentType));

        // Packager-Pack
        if (templateDesc is not null)
        {
            _reflect.Packagers.Pack(templateDesc, eventDesc.Instance, context);
        }
        // Before-Rules
        foreach (var ruleDesc in _rules.AllBefore)
        {
            if (!_reflect.Rules.ShouldApply(ruleDesc, context))
            {
                continue;
            }
            var event2 = _reflect.Rules.Apply(ruleDesc, context);
            var event2Desc = plugin.Registry.GetEventDescriptor(event2);
            Queue.Enqueue(event2Desc);
        }
        // Effect-Apply
        if (effectDesc is not null)
        {
            if (_reflect.Effects.ShouldApply(effectDesc, eventDesc.Instance, pipeline))
            {
                _reflect.Effects.Apply(effectDesc, eventDesc.Instance, pipeline);
            }
        }
        // After-Rules
        foreach (var ruleDesc in _rules.AllAfter)
        {
            if (!_reflect.Rules.ShouldApply(ruleDesc, context))
            {
                continue;
            }
            var event2 = _reflect.Rules.Apply(ruleDesc, context);
            var event2Desc = plugin.Registry.GetEventDescriptor(event2);
            Queue.Enqueue(event2Desc);
        }

        foreach (var item in eventDesc.Continuations)
        {
            var event2 = item.DynamicInvoke(eventDesc.Instance).As<IEvent>();
            var event2Desc = plugin.Registry.GetEventDescriptor(event2);
            Queue.Enqueue(event2Desc);
        }

        _rules.Clear();
    }
}

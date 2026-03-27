namespace RpgCompanion.Application.Engines;

using Core.Engine;
using Core.Events;

internal class Trigger(EventQueue queue, PluginManager manager) : ITrigger
{
    public IPipeline<TEvent> Start<TEvent>(TEvent e) where TEvent : IEvent
    {
        var plugin = manager[e.GetType()];
        var pipeline = new Pipeline(queue, plugin.Registry);
        return pipeline.Raise(e);
    }
}

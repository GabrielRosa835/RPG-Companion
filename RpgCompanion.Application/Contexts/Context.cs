using RpgCompanion.Application;
using RpgCompanion.Core.Contexts;
using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;

namespace RpgCompanion.Application;

internal class Context (Engine engine, PluginDescriptor plugin) : IContext, ISnapshot
{
   internal readonly Engine _engine = engine;
   internal readonly ContextData _data = new();
   internal readonly ComponentProvider _registry = plugin.Registry;

   public IContextData Data => _data;
   ISnapshotData ISnapshot.Data => _data;
   public IRegistry Registry => _registry;

   public void Raise (IEvent @event)
   {
      var descriptor = _registry.GetEventDescriptor(@event);
      _engine._queue.Enqueue(descriptor);
   }
}
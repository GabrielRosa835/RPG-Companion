using RpgCompanion.Application;
using RpgCompanion.Core.Contexts;
using RpgCompanion.Core.Events;

namespace RpgCompanion.Core.Engine;

internal class Context (PluginDescriptor plugin) : IContext, ISnapshot
{
   internal readonly ContextData _data = new();
   internal readonly ComponentProvider _registry = plugin.Registry;

   public IContextData Data => _data;
   ISnapshotData ISnapshot.Data => _data;
   public IRegistry Registry => _registry;
   IReadOnlyCollection<IEvent> ProcessedEvents { get; } = [];
   IReadOnlyCollection<IEffect<IEvent>> ProcessedEffects { get; } = [];


   public void Raise (IEvent @event)
   {

   }
}
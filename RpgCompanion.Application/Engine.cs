using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Events.Producers;

namespace RpgCompanion.Application;

internal class Engine
{
   private readonly Dictionary<Type, PluginDescriptor> _events = default!;
   private readonly Dictionary<string, PluginDescriptor> _plugins = default!;
   private readonly PluginManager _pluginManager = default!;
   private readonly EventQueue _queue = default!;

   public async Task Execute(PluginDescriptor plugin)
   {
      while(!_queue.Any())
      {
         await Task.Delay(100);
      }

      if (!plugin.Activated)
      {
         var loadAttempt = _pluginManager.Load(plugin);
         if (loadAttempt.IsFailure)
         {
            return;
         }
      }

      var contextProvider = plugin.Registry.Get<IContextProvider>() ?? default!;

      IEvent @event = _queue.Dequeue();
      Context context = contextProvider.Bundle(@event);

      // TODO: Reflection
      IEnumerable<IInterceptor> interceptors = plugin.Registry.GetInterceptorsFor(@event);
      ICollection<IEvent> intercepted = [];
      foreach (IInterceptor interceptor in interceptors)
      {
         intercepted.Add(interceptor.Apply(@event, context));
      }
      if (intercepted.Count > 0)
      {
         @event = new CompositeEvent(intercepted);
      }

      IEnumerable<IEventHandler> handlers = plugin.Registry.GetHandlersFor(@event);
      foreach (IEventHandler handler in handlers)
      {
         handler.Handle(@event, context);
      }
   }
}
using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Events.Producers;

namespace RpgCompanion.Application;

public class Engine
{
   private readonly IContextProvider _contextProvider;
   private readonly IEventQueue _eventQueue;
   private readonly IRegistry _registry;

   public Engine (IContextProvider contextProvider, IEventQueue eventQueue, IRegistry registry)
   {
      _contextProvider = contextProvider;
      _eventQueue = eventQueue;
      _registry = registry;
   }

   public async Task ExecuteLoop()
   {
      while(!_eventQueue.Any())
      {
         await Task.Delay(100);
      }

      IEvent @event = _eventQueue.Pop();
      IContext context = _contextProvider.Bundle(@event);

      IEnumerable<IInterceptor> interceptors = _registry.GetInterceptorsFor(@event);
      ICollection<IEvent> intercepted = [];
      foreach (IInterceptor interceptor in interceptors)
      {
         intercepted.Add(interceptor.Apply(@event, context));
      }
      if (intercepted.Count > 0)
      {
         @event = new CompositeEvent(intercepted);
      }

      IEnumerable<IEventHandler> handlers = _registry.GetHandlersFor(@event);
      foreach (IEventHandler handler in handlers)
      {
         handler.Handle(@event, context);
      }
   }
}
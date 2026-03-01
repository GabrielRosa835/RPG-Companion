using Microsoft.Extensions.DependencyInjection;

using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Events.Producers;
using RpgCompanion.Core.Meta;
using RpgCompanion.Core.Utils;

namespace RpgCompanion.Application;

public class Registry(IServiceProvider provider) : IRegistry
{
   public T? Get<T> () => throw new NotImplementedException();
   public T GetRequired<T> () => throw new NotImplementedException();

   public IEnumerable<IInterceptor> GetInterceptorsFor (IEvent @event)
   {
      Type eventType = @event.GetType();
      Type validInterceptors = typeof(IInterceptor<>).MakeGenericType(eventType);
      var interceptors = provider.GetServices(validInterceptors);
      return interceptors.As<IEnumerable<IInterceptor>>();
   }
   public IEnumerable<IEventHandler> GetHandlersFor (IEvent @event)
   {
      Type eventType = @event.GetType();
      Type validHandlers = typeof(IEventHandler<>).MakeGenericType(eventType);
      var handlers = provider.GetServices(validHandlers);
      return handlers.As<IEnumerable<IEventHandler>>();
   }

   public IEnumerable<object?> GetServices (Type serviceType)
   {
      return provider.GetServices(serviceType);
   }
}
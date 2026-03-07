using Microsoft.Extensions.DependencyInjection;

using RpgCompanion.Application.Services;
using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Events.Producers;

namespace RpgCompanion.Application;

internal class ComponentProvider
{
   private readonly IReadOnlyList<ComponentDescriptor> _components;
   private readonly IServiceProvider _provider;

   internal ComponentProvider(IEnumerable<ComponentDescriptor> components, IServiceProvider provider) 
   {
      _components = new List<ComponentDescriptor>(components).AsReadOnly();
      _provider = provider;
   }

   public T GetRequired<T> () where T : notnull => _provider.GetRequiredService<T>();
   public T? Get<T> () where T : notnull => _provider.GetService<T>();


   internal TRule? GetRule<TRule, TEvent> () where TEvent : IEvent where TRule : class, IRule<TEvent>
   {
      var descriptor = _components.FirstOrDefault(d =>
         d.Category == ComponentCategory.Rule
         && d.ComponentType == typeof(TRule)
         && d.EventType == typeof(TEvent));
      return Get<TRule>(descriptor);
   }
   internal PriorityQueue<object, int> GetEffects<TEvent> () where TEvent : IEvent
   {
      var effects = _components.Where(d =>
         d.Category == ComponentCategory.Effect
         && d.EventType == typeof(TEvent))
         .Select(d => (
            Effect: _provider.GetService(d.ComponentType)!,
            Priority: int.MaxValue - (d.Effect_Priority ?? 0)
         ));
      return new PriorityQueue<object, int>(effects);
   }
   internal int GetPriorityFor<TEffect>()
   {
      return _components.FirstOrDefault(d => d.ComponentType == typeof(TEffect))?.Effect_Priority ?? 0;
   }

   internal object? GetTemplate(IEvent @event)
   {
      var templateType = typeof(IContextTemplate<>).MakeGenericType(@event.GetType());
      var template = _provider.GetService(templateType);
      return template;
   }
   internal object? GetContract (IEvent @event)
   {
      var contractType = typeof(IEventContract<>).MakeGenericType(@event.GetType());
      var contract = _provider.GetService(contractType);
      return contract;
   }

   private T? Get<T>(ComponentDescriptor? descriptor) where T : class
   {
      if (descriptor is null)
      {
         return null!;
      }
      if (descriptor.ComponentInstance is not null)
      {
         return (T) descriptor.ComponentInstance;
      }
      return (T?) _provider.GetService(descriptor.ComponentType);
   }
}
using Microsoft.Extensions.DependencyInjection;

using RpgCompanion.Application.Services;

namespace RpgCompanion.Application;

internal class ComponentProvider
{
   private readonly IReadOnlyList<ComponentDescriptor> _components;
   private readonly IServiceProvider _provider;

   internal ComponentProvider (IEnumerable<ComponentDescriptor> components, IServiceProvider provider)
   {
      _components = new List<ComponentDescriptor>(components).AsReadOnly();
      _provider = provider;
   }

   internal ComponentDescriptor? GetTemplate (Type eventType)
   {
      var descriptor = _components.FirstOrDefault(d =>
         d.Category == ComponentCategory.Packager &&
         d.EventType == eventType);
      if (descriptor is null) return null;
      var template = _provider.GetRequiredService(descriptor.GenericType);
      descriptor.Instance = template;
      return descriptor;
   }
   internal ComponentDescriptor? GetContract (Type eventType)
   {
      var descriptor = _components.FirstOrDefault(d =>
         d.Category == ComponentCategory.Contract &&
         d.EventType == eventType);
      if (descriptor is null) return null;
      var contract = _provider.GetRequiredService(descriptor.GenericType);
      descriptor.Instance = contract;
      return descriptor;
   }
   internal ComponentDescriptor? GetEffect (Type eventType)
   {
      var descriptor = _components.FirstOrDefault(d =>
         d.Category == ComponentCategory.Effect &&
         d.EventType == eventType);
      if (descriptor is null) return null;
      var effect = _provider.GetRequiredService(descriptor.GenericType);
      descriptor.Instance = effect;
      return descriptor;
   }
   internal IEnumerable<ComponentDescriptor> GetRules (Type eventType)
   {
      var ruleDescriptors = _components.Where(d =>
         d.Category == ComponentCategory.Rule &&
         d.EventType == eventType).ToArray();
      if (ruleDescriptors.Length > 0) return [];
      return _provider.GetServices(ruleDescriptors.First().GenericType)
         .Where(s => s is not null)
         .Select (s =>
         {
            var descriptor = ruleDescriptors.First(d => d.ComponentType == s!.GetType());
            descriptor.Instance = s!;
            return descriptor;
         })
         .ToList();
   }
}
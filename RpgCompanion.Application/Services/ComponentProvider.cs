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

   internal object? GetTemplate (Type eventType)
   {
      var descriptor = FindTemplateFor(eventType);
      if (descriptor is null) return null;
      var template = _provider.GetService(descriptor.GenericType);
      return template;
   }
   internal object? GetContract (Type eventType)
   {
      var descriptor = FindContractFor(eventType);
      if (descriptor is null) return null;
      var contract = _provider.GetService(descriptor.GenericType);
      return contract;
   }
   internal ICollection<(object Rule, RulePlacement Placement)> GetRules (Type eventType)
   {
      var ruleDescriptors = FindRulesFor(eventType);
      if (!ruleDescriptors.Any()) return [];
      return _provider.GetServices(ruleDescriptors.First().GenericType)
         .Where(s => s is not null)
         .Select(s => (s!, ruleDescriptors.First(d => d.ComponentType == s!.GetType()).Rule_Placement!.Value))
         .ToList();
   }
   internal ICollection<(object Effect, int Priority)> GetEffects (Type eventType)
   {
      var effectDescriptors = FindEffectsFor(eventType);
      if (!effectDescriptors.Any()) return [];
      return _provider.GetServices(effectDescriptors.First().GenericType)
         .Where(s => s is not null)
         .Select(s => (s!, effectDescriptors.First(d => d.ComponentType == s!.GetType()).Effect_Priority!.Value))
         .ToList();
   }


   private IEnumerable<ComponentDescriptor> FindRulesFor (Type eventType)
   {
      return _components.Where(d =>
        d.Category == ComponentCategory.Rule &&
        d.EventType == eventType);
   }
   private IEnumerable<ComponentDescriptor> FindEffectsFor (Type eventType)
   {
      return _components.Where(d =>
        d.Category == ComponentCategory.Effect &&
        d.EventType == eventType);
   }
   private ComponentDescriptor? FindContractFor (Type eventType)
   {
      return _components.FirstOrDefault(d =>
        d.Category == ComponentCategory.Contract &&
        d.EventType == eventType);
   }
   private ComponentDescriptor? FindTemplateFor (Type eventType)
   {
      return _components.FirstOrDefault(d =>
        d.Category == ComponentCategory.Template &&
        d.EventType == eventType);
   }
}
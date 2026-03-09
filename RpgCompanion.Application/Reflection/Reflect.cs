using RpgCompanion.Core.Contexts;
using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Events.Producers;

using System.Reflection;

namespace RpgCompanion.Application.Reflection;

internal class Reflect
{
   private readonly Dictionary<Type, MethodInfo> _methods = [];
   private readonly Dictionary<Type, PropertyInfo> _properties = [];
   private readonly Timer _cleanupTimer;

   internal Reflect()
   {
      _cleanupTimer = new Timer(Clear, this, TimeSpan.Zero, TimeSpan.FromMilliseconds(1000));
   }

   internal MethodInfo RuleApply(Type genericType)
   {
      return GetMethod(genericType, typeof(IRule<>), nameof(IRule<>.Apply));
   }
   internal MethodInfo EffectApply (Type genericType)
   {
      return GetMethod(genericType, typeof(IEffect<>), nameof(IEffect<>.Apply));
   }
   internal MethodInfo TemplateBundle (Type genericType)
   {
      return GetMethod(genericType, typeof(IContextTemplate<>), nameof(IContextTemplate<>.Bundle));
   }
   internal PropertyInfo ContractRequirements(Type genericType)
   {
      return GetProperty(genericType, typeof(IEventContract<>), nameof(IEventContract<>.Requirements));
   }
   internal PropertyInfo ContractOutputs (Type genericType)
   {
      return GetProperty(genericType, typeof(IEventContract<>), nameof(IEventContract<>.Outputs));
   }

   private MethodInfo GetMethod(Type genericType, Type genericDefinition, string methodName)
   {
      if (_methods.TryGetValue(genericType, out var existant))
      {
         return existant;
      }
      if (!genericType.GetGenericTypeDefinition().IsAssignableTo(genericDefinition))
      {
         throw new InvalidOperationException();
      }
      var method = genericType.GetMethod(methodName)!;
      _methods.Add(genericType, method);
      return method;
   }
   private PropertyInfo GetProperty (Type genericType, Type genericDefinition, string propertyName)
   {
      if (_properties.TryGetValue(genericType, out var existant))
      {
         return existant;
      }
      if (!genericType.GetGenericTypeDefinition().IsAssignableTo(genericDefinition))
      {
         throw new InvalidOperationException();
      }
      var property = genericType.GetProperty(propertyName)!;
      _properties.Add(genericType, property);
      return property;
   }
   private void Clear (object? state)
   {
      Reflect r = (Reflect) state!;
      _methods.Clear();
      _properties.Clear();
   }
}

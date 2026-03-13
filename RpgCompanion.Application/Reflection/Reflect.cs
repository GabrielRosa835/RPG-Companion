using RpgCompanion.Core.Contexts;
using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Events.Producers;

using System.Reflection;
using RpgCompanion.Application.Services;

namespace RpgCompanion.Application.Reflection;

internal class Reflect : IReflect, IReflectEffect, IReflectContract, IReflectPackager
{
   private readonly Dictionary<Type, MethodInfo> _methods = [];
   private readonly Dictionary<Type, PropertyInfo> _properties = [];
   private readonly Timer _cleanupTimer;

   internal Reflect()
   {
      _cleanupTimer = new Timer(Clear, this, TimeSpan.Zero, TimeSpan.FromMilliseconds(1000));
   }

   public IReflect Rules => this;
   public IReflectEffect Effects => this;
   public IReflectContract Contracts => this;
   public IReflectPackager Packagers => this;

   #region Implementations
   
   public bool ShouldApply(RuleDescriptor descriptor, ISnapshot snapshot)
   {
      return GetMethod(descriptor.GenericType, nameof(IRule<>.ShouldApply))
         .Invoke(descriptor.Instance, [snapshot])!.As<bool>();
   }
   public IEvent Apply(RuleDescriptor descriptor, ISnapshot snapshot)
   {
      return GetMethod(descriptor.GenericType, nameof(IRule<>.Apply))
         .Invoke(descriptor.Instance, [snapshot])!.As<IEvent>();
   }
   public bool ShouldApply (EffectDescriptor descriptor, IEvent @event, IContext context)
   {
      return GetMethod(descriptor.GenericType, nameof(IEffect<>.ShouldApply))
         .Invoke(descriptor.Instance, [@event, context])!.As<bool>();
   }
   public void Apply (EffectDescriptor descriptor, IEvent @event, IContext context)
   {
      GetMethod(descriptor.GenericType, nameof(IEffect<>.Apply))
         .Invoke(descriptor.Instance, [@event, context]);
   }
   public void Pack (PackagerDescriptor descriptor, IEvent @event, IContext context)
   {
      GetMethod(descriptor.GenericType, nameof(IPackager<>.Pack))
         .Invoke(descriptor.Instance, [@event, context]);
   }
   public IEnumerable<ContextKey> Requirements(ContractDescriptor descriptor)
   {
      return GetProperty(descriptor.GenericType, nameof(IContract<>.Requirements))!
         .GetValue(descriptor.Instance)!.As<IEnumerable<ContextKey>>();
   }
   public IEnumerable<ContextKey> Outputs (ContractDescriptor descriptor)
   {
      return GetProperty(descriptor.GenericType, nameof(IContract<>.Outputs))!
         .GetValue(descriptor.Instance)!.As<IEnumerable<ContextKey>>();
   }

   #endregion
   
   private MethodInfo GetMethod(Type genericType, string methodName)
   {
      if (_methods.TryGetValue(genericType, out var existant))
      {
         return existant;
      }
      var method = genericType.GetMethod(methodName)!;
      _methods.Add(genericType, method);
      return method;
   }
   private PropertyInfo GetProperty (Type genericType, string propertyName)
   {
      if (_properties.TryGetValue(genericType, out var existant))
      {
         return existant;
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
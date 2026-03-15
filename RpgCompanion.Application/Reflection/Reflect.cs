namespace RpgCompanion.Application.Reflection;

using System.Reflection;
using RpgCompanion.Application.Services;
using RpgCompanion.Core.Contexts;
using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Events.Producers;

internal class Reflect : IReflect, IReflectEffect, IReflectPackager, IDisposable
{
    private readonly Dictionary<Type, MethodInfo> _methods = [];
    private readonly Dictionary<Type, PropertyInfo> _properties = [];
    private readonly Timer _cleanupTimer;

    internal Reflect() => _cleanupTimer = new Timer(Clear, this, TimeSpan.Zero, TimeSpan.FromMilliseconds(1000));

    public IReflect Rules => this;
    public IReflectEffect Effects => this;
    public IReflectPackager Packagers => this;

    #region Implementations

    public bool ShouldApply(RuleDescriptor descriptor, IContext snapshot)
    {
        return GetMethod(descriptor.GenericType, nameof(IRule<>.ShouldApply))
           .Invoke(descriptor.Instance, [snapshot])!.As<bool>();
    }
    public IEvent Apply(RuleDescriptor descriptor, IContext snapshot)
    {
        return GetMethod(descriptor.GenericType, nameof(IRule<>.Apply))
           .Invoke(descriptor.Instance, [snapshot])!.As<IEvent>();
    }
    public bool ShouldApply(EffectDescriptor descriptor, IEvent e, IPipeline pipeline)
    {
        return GetMethod(descriptor.GenericType, nameof(IEffect<>.ShouldApply))
           .Invoke(descriptor.Instance, [e, pipeline])!.As<bool>();
    }
    public void Apply(EffectDescriptor descriptor, IEvent e, IPipeline pipeline)
    {
        GetMethod(descriptor.GenericType, nameof(IEffect<>.Apply))
           .Invoke(descriptor.Instance, [e, pipeline]);
    }
    public void Pack(PackagerDescriptor descriptor, IEvent e, IEditableContext context)
    {
        GetMethod(descriptor.GenericType, nameof(IPackager<>.Pack))
           .Invoke(descriptor.Instance, [e, context]);
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
    private PropertyInfo GetProperty(Type genericType, string propertyName)
    {
        if (_properties.TryGetValue(genericType, out var existant))
        {
            return existant;
        }
        var property = genericType.GetProperty(propertyName)!;
        _properties.Add(genericType, property);
        return property;
    }
    private void Clear(object? state)
    {
        var r = (Reflect) state!;
        _methods.Clear();
        _properties.Clear();
    }

    public void Dispose()
    {
        _cleanupTimer?.Dispose();
    }
}

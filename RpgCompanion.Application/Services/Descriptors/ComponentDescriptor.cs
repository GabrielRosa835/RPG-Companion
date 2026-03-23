using Microsoft.Extensions.DependencyInjection;

namespace RpgCompanion.Application.Services;

using System.Reflection;
using Reflection;

internal abstract class ComponentDescriptor
{
    public ServiceDescriptor? Service { get; set; }
    public object? Instance { get; set; }
    public bool HasService => this.Service is not null;

    protected object? InvokeMethod(Reflect reflect, string methodName, Delegate? substitute = null, params object?[]? args)
    {
        if (substitute is not null)
        {
            return substitute.DynamicInvoke(args);
        }
        if (this.HasService)
        {
            if (this.Instance is null)
            {
                throw new InvalidOperationException($"Cannot invoke {methodName} on a null instance");
            }
            MethodInfo? method = reflect.GetMethod(this.Service!.ServiceType, methodName);
            if (method is not null)
            {
                return method.Invoke(this.Instance, args);
            }
        }
        throw new InvalidOperationException($"Cannot invoke {methodName} of {this.Service!.ServiceType.Name}");
    }
}

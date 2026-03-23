namespace RpgCompanion.Application;

using Core.Contexts;
using Core.Events;
using Core.Meta;
using Microsoft.Extensions.DependencyInjection;
using Services;

internal class BundlerBuilder<TEvent>(IServiceCollection services) : IBundlerBuilder<TEvent> where TEvent : IEvent
{
    private BundlerDescriptor _descriptor = new();
    public BundlerDescriptor Build()
    {
        return _descriptor;
    }

    public IBundlerBuilder<TEvent> WithComponent<TBundler>() where TBundler : class, IBundler<TEvent>
    {
        services.AddTransient<TBundler>();
        services.AddTransient<IBundler<TEvent>, TBundler>();
        var serviceDescriptor = services.Last();
        _descriptor.Service = serviceDescriptor;
        return this;
    }

    public IBundlerBuilder<TEvent> WithAction(BundlerAction<TEvent> action)
    {
        _descriptor.Action = action;
        return this;
    }
}

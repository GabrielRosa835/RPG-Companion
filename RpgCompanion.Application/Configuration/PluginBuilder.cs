namespace RpgCompanion.Application;

using Core.Engine;
using Core.Events;
using Core.Meta;
using Microsoft.Extensions.DependencyInjection;

internal class PluginBuilder(IServiceCollection services) : IPluginBuilder
{
    private PluginDefinition _definition = new();
    public PluginDefinition Build()
    {
        _definition.Services = services.BuildServiceProvider();
        return _definition;
    }

    public IPluginBuilder AddEvent<TEvent>(Action<IEventBuilder<TEvent>> configure) where TEvent : IEvent
    {
        var builder = new EventBuilder<TEvent>(services, _definition.Components);
        configure(builder);
        _definition.Components.Events.Add(builder.Build());
        return this;
    }

    public IPluginBuilder WithMetadata(Action<IPluginMetadataBuilder> configure)
    {
        var builder = new PluginMetadataBuilder(services);
        configure(builder);
        _definition.Metadata = builder.Build();
        return this;
    }
}

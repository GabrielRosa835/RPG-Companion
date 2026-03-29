using RpgCompanion.Core.Events;
using RpgCompanion.Core.Meta;

namespace RpgCompanion.Application;

using Microsoft.Extensions.DependencyInjection;
using Services;

// Transient
internal class PluginBuilder(IServiceProvider hostServices) : IPluginBuilder
{
    private readonly IServiceCollection _services = new ServiceCollection();
    private readonly ComponentCollection _components = new();
    private readonly PluginMetadata _metadata = new();
    public PluginDefinition Build()
    {
        _services.AddPluginServices(hostServices, _components);
        return new ()
        {
            Components = _components,
            Metadata = _metadata,
            Services = _services.BuildServiceProvider(),
        };
    }

    public IPluginBuilder AddEvent<TEvent>(Action<IEventBuilder<TEvent>> configure) where TEvent : IEvent
    {
        var builder = new EventBuilder<TEvent>(_services, _components);
        configure(builder);
        _components.Events.Add(builder.Build());
        return this;
    }

    public IPluginBuilder WithMetadata(Action<IPluginMetadataBuilder> configure)
    {
        var builder = new PluginMetadataBuilder(_metadata);
        configure(builder);
        return this;
    }

    public IPluginBuilder WithInitialization(Action<IInitializationBuilder> configure)
    {
        var builder = new InitializationBuilder(_services);
        configure(builder);
        return this;
    }
}

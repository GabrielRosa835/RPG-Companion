namespace RpgCompanion.QuartzPrototype.Configuration;

using Microsoft.Extensions.DependencyInjection;
using Plugins;
using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Engine.Contexts;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Meta;

// Transient
internal class PluginBuilder(IServiceCollection services) : IPluginBuilder
{
    private readonly ComponentCollection _components = new();
    private readonly PluginMetadata _metadata = new();
    public PluginDefinition Build()
    {
        services.AddSingleton<ContextData>();
        services.AddSingleton(_components);

        services.AddScoped<EventExecutor>();
        services.AddScoped<Context>();

        services.AddTransient<ComponentProvider>();
        services.AddTransient<Registry>();
        services.AddTransient<Pipeline>();

        services.AddTransient<IRegistry>(sp => sp.GetRequiredService<Registry>());
        services.AddTransient<IPipeline>(sp => sp.GetRequiredService<Pipeline>());
        services.AddTransient<IContext>(sp => sp.GetRequiredService<Context>());

        return new ()
        {
            Components = _components,
            Metadata = _metadata,
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

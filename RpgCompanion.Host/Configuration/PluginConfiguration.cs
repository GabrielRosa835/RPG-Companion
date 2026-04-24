namespace RpgCompanion.Host.Configuration;

using Core;
using Core.Events;
using Descriptors;
using Microsoft.Extensions.DependencyInjection;
using Plugins;

// Transient
internal class PluginConfiguration(IServiceCollection _services, PluginKey _key, PluginMetadata _metadata)
{
    private string? _name;
    private string? _version;
    private HashSet<EventKey> _events = [];

    public PluginDescriptor Build()
    {
        var descriptor = new PluginDescriptor
        {
            Key =  _key,
            Version =  _version,
            Events = _events,
            Metadata = _metadata,
        };
        _services.AddKeyedSingleton(_key, descriptor);
        return descriptor;
    }

    public PluginConfiguration WithName(string name)
    {
        _name = name;
        return this;
    }

    public PluginConfiguration WithVersion(string version)
    {
        _version = version;
        return this;
    }

    public PluginConfiguration AddEvent<TEvent>(string key, Action<EventConfiguration<TEvent>> configure) where TEvent : IEvent
        => AddEventInternal(new(key), configure);

    public PluginConfiguration AddEvent<TEvent>(Action<EventConfiguration<TEvent>> configure) where TEvent : IEvent
        => AddEventInternal(new(), configure);

    private PluginConfiguration AddEventInternal<TEvent>(EventKey eventKey, Action<EventConfiguration<TEvent>> configure) where TEvent : IEvent
    {
        var builder = new EventConfiguration<TEvent>(_services, eventKey);
        configure(builder);
        _events.Add(eventKey);
        return this;
    }

    public PluginConfiguration WithInitialization(Action<InitializationConfiguration> configure)
    {
        var builder = new InitializationConfiguration(_services, _key);
        configure(builder);
        return this;
    }
}

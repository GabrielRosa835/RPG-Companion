namespace RpgCompanion.Host.Configuration;

using Core;

internal class PluginConfiguration(
    IServiceCollection _services,
    PluginMetadata _metadata)
    : IPluginConfiguration
{
    private readonly PluginKey _key = new();
    private readonly HashSet<EffectKey> _effects = [];
    private readonly HashSet<ConditionKey> _conditions = [];
    private readonly HashSet<RuleKey> _rules = [];
    private readonly HashSet<EventKey> _events = [];
    private readonly HashSet<ActorKey> _actors = [];
    private InitializationKey? _initialization;
    private string? _name;
    private string? _version;

    public PluginDescriptor Build()
    {
        var descriptor = new PluginDescriptor
        {
            Key = _key,
            Name = _name,
            Version = _version,
            Metadata = _metadata,
            Events = _events,
            Conditions = _conditions,
            Effects = _effects,
            Rules = _rules,
            Actors = _actors,
            Initialization = _initialization,
        };
        _services.AddKeyedSingleton(_key, descriptor);
        return descriptor;
    }

    public IPluginConfiguration WithName(string name)
    {
        _name = name;
        return this;
    }

    public IPluginConfiguration WithVersion(string version)
    {
        _version = version;
        return this;
    }

    public IPluginConfiguration AddActor<TActor>(Action<IActorConfiguration<TActor>> configure) where TActor : class
    {
        var configuration = new ActorConfiguration<TActor>(_services, _key, _rules, _effects);
        configure(configuration);
        var descriptor = configuration.Build();
        _actors.Add(descriptor.Key);
        return this;
    }

    public IPluginConfiguration AddEvent<TEvent>(Action<IEventConfiguration<TEvent>> configure) where TEvent : IEvent
    {
        var configuration = new EventConfiguration<TEvent>(_services, _key, _rules, _effects);
        configure(configuration);
        var descriptor = configuration.Build();
        _events.Add(descriptor.Key);
        return this;
    }

    public IPluginConfiguration WithInitialization(Action<IInitializationConfiguration> configure)
    {
        var configuration = new InitializationConfiguration(_services, _key);
        configure(configuration);
        var descriptor = configuration.Build();
        _initialization = descriptor.Key;
        return this;
    }
}

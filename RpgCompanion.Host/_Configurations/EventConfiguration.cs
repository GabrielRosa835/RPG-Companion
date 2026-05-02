namespace RpgCompanion.Host;

using Core;
using Microsoft.AspNetCore.Mvc;

internal class EventConfiguration<TEvent>(
    IServiceCollection _services,
    PluginKey _plugin,
    ISet<RuleKey> _pluginRules)
    : IEventConfiguration<TEvent>
    where TEvent : class, IEvent
{
    private readonly List<Action> _lazyConfigurations = [];
    private readonly HashSet<RuleKey> _actions = [];
    private readonly HashSet<RuleKey> _rules = [];
    private EventKey? _key;
    private string? _displayName;
    private string? _description;

    internal EventKey Build()
    {
        KeyException.ThrowIfNull(_key);
        foreach (Action lazyConfiguration in _lazyConfigurations)
        {
            lazyConfiguration.Invoke();
        }
        var descriptor = new EventDescriptor
        {
            Key = _key.Value,
            DisplayName = _displayName,
            Description = _description,
            Type = typeof(TEvent),
            Connections = new()
            {
                Plugin = _plugin,
                Actions = _actions,
                Rules = _rules,
            }
        };
        _services.AddKeyedSingleton(_key, descriptor);
        _services.AddSingleton(descriptor);
        return _key.Value;
    }

    public IEventConfiguration<TEvent> WithKey(EventKey<TEvent> key)
    {
        _key = key;
        return this;
    }

    public IEventConfiguration<TEvent> WithName(string name)
    {
        _displayName = name;
        return this;
    }

    public IEventConfiguration<TEvent> WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public IEventConfiguration<TEvent> AddRule(Configure<IRuleConfiguration<TEvent>> configure)
    {
        _lazyConfigurations.Add(() =>
        {
            var configuration = new RuleConfiguration<TEvent>(
                _services: _services,
                _plugin: _plugin,
                _pluginRules: _pluginRules,
                _event: _key,
                _actor: null);
            configure(configuration);
            RuleKey key = configuration.Build();
            _pluginRules.Add(key);
            _rules.Add(key);
        });
        return this;
    }

    public IEventConfiguration<TEvent> AddRule<U>(Configure<IRuleConfiguration<TEvent, U>> configure)
    {
        _lazyConfigurations.Add(() =>
        {
            var configuration = new RuleConfiguration<TEvent, U>(
                _services: _services,
                _plugin: _plugin,
                _pluginRules: _pluginRules,
                _event: _key,
                _actor: null);
            configure(configuration);
            RuleKey key = configuration.Build();
            _pluginRules.Add(key);
            _rules.Add(key);
        });
        return this;
    }

    public IEventConfiguration<TEvent> AddAction<TOtherEvent>(
        Configure<IActionConfiguration<TEvent, TOtherEvent>> configure) where TOtherEvent : class, IEvent
    {
        _lazyConfigurations.Add(() =>
        {
            var configuration = new ActionConfiguration<TEvent, TOtherEvent>(
                _services: _services,
                _plugin: _plugin,
                _pluginRules: _pluginRules,
                _actor: null,
                _event: _key);
            configure(configuration);
            var key = configuration.Build();
            _actions.Add(key);
            _pluginRules.Add(key);
        });
        return this;
    }
}

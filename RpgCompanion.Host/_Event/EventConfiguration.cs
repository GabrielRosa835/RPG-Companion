namespace RpgCompanion.Host;

using Core;
using Microsoft.AspNetCore.Mvc;

internal class EventConfiguration<TEvent>(
    IServiceCollection _services,
    PluginKey _plugin,
    ISet<RuleKey> _pluginRules)
    : IEventConfiguration<TEvent>
    where TEvent : IEvent
{
    private readonly List<Action> _lazyConfigurations = [];
    private readonly HashSet<RuleKey> _rules = [];
    private EventKey? _key;
    private string? _displayName;
    private int? _priority;

    internal EventKey Build()
    {
        KeyException.ThrowIfNull(_key);
        foreach (Action lazyConfiguration in _lazyConfigurations)
        {
            lazyConfiguration();
        }
        var descriptor = new EventDescriptor
        {
            Key = _key.Value,
            Plugin = _plugin,
            Rules = _rules,
            DisplayName = _displayName,
            Priority = _priority ?? 0,
        };
        _services.AddKeyedSingleton(_key, descriptor);
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
                _conditionFor: null,
                _event: _key,
                _actor: null);
            configure(configuration);
            RuleKey key = configuration.Build();
            _pluginRules.Add(key);
            _rules.Add(key);
        });
        return this;
    }
}

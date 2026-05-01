namespace RpgCompanion.Host;

using Core;
using Microsoft.Extensions.DependencyInjection.Extensions;

public class ActorConfiguration<TActor>(
    IServiceCollection _services,
    PluginKey _plugin,
    ISet<RuleKey> _pluginRules)
    : IActorConfiguration<TActor> where TActor : class, IActor
{
    private readonly List<Action> _lazyConfigurations = [];
    private readonly HashSet<RuleKey> _rules = [];
    private Lifetime? _lifetime;
    private ActorKey? _key;
    private string? _displayName;

    public ActorKey Build()
    {
        KeyException.ThrowIfNull(_key);
        foreach (Action lazyConfiguration in _lazyConfigurations)
        {
            lazyConfiguration();
        }
        _services.AddKeyedSingleton(_key, new ActorDescriptor
        {
            Key = _key.Value,
            Plugin = _plugin,
            DisplayName = _displayName,
            Rules = _rules,
        });
        return _key.Value;
    }

    public IActorConfiguration<TActor> WithKey(ActorKey<TActor> key)
    {
        _key = key;
        return this;
    }

    public IActorConfiguration<TActor> WithLifetime(Lifetime lifetime)
    {
        _lifetime = lifetime;
        return this;
    }

    public IActorConfiguration<TActor> WithName(string name)
    {
        _displayName = name;
        return this;
    }

    public IActorConfiguration<TActor> Export()
    {
        _lazyConfigurations.Add(() =>
        {
            if (_lifetime == Lifetime.Persistent)
            {
                _services.TryAddKeyedSingleton<TActor>(_key);
                _services.TryAddSingleton<TActor>();
                return;
            }
            // Defaults to transient
            _services.TryAddKeyedTransient<TActor>(_key);
            _services.TryAddTransient<TActor>();
        });
        return this;
    }

    public IActorConfiguration<TActor> AddRule<U>(Configure<IRuleConfiguration<TActor, U>> configure)
    {
        _lazyConfigurations.Add(() =>
        {
            var configuration = new RuleConfiguration<TActor, U>(
                _services: _services,
                _plugin: _plugin,
                _pluginRules: _pluginRules,
                _conditionFor: null,
                _event: null,
                _actor: _key);
            configure(configuration);
            RuleKey key = configuration.Build();
            _pluginRules.Add(key);
            _rules.Add(key);
        });
        return this;
    }

    public IActorConfiguration<TActor> AddRule(Configure<IRuleConfiguration<TActor>> configure)
    {
        _lazyConfigurations.Add(() =>
        {
            var configuration = new RuleConfiguration<TActor>(
                _services: _services,
                _plugin: _plugin,
                _pluginRules: _pluginRules,
                _event: null,
                _actor: _key);
            configure(configuration);
            RuleKey key = configuration.Build();
            _pluginRules.Add(key);
            _rules.Add(key);
        });
        return this;
    }
}

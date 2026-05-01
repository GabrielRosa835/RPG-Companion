namespace RpgCompanion.Host;

using Core;
using Microsoft.Extensions.DependencyInjection.Extensions;

internal class RuleConfiguration<T>(
    IServiceCollection _services,
    PluginKey _plugin,
    ISet<RuleKey> _pluginRules,
    EventKey? _event,
    ActorKey? _actor)
    : IRuleConfiguration<T>
{
    private readonly List<Action> _lazyConfigurations = [];
    private RuleKey? _key;
    private RuleKey? _condition;
    private string? _displayName;

    internal RuleKey Build()
    {
        KeyException.ThrowIfNull(_key);
        foreach (Action lazyConfiguration in _lazyConfigurations)
        {
            lazyConfiguration();
        }
        _services.AddKeyedSingleton(_key, new RuleDescriptor
        {
            Key = _key.Value,
            Plugin = _plugin,
            Event = _event,
            Actor = _actor,
            Condition = _condition,
            DisplayName = _displayName,
        });
        return _key.Value;
    }

    public IRuleConfiguration<T> WithKey(RuleKey<T> key)
    {
        _key = key;
        return this;
    }

    public IRuleConfiguration<T> WithName(string name)
    {
        _displayName = name;
        return this;
    }

    public IRuleConfiguration<T> WithCondition(Configure<IRuleConfiguration<T, bool>> configure)
    {
        _lazyConfigurations.Add(() =>
        {
            var configuration = new RuleConfiguration<T, bool>(
                _services: _services,
                _plugin: _plugin,
                _pluginRules: _pluginRules,
                _conditionFor: _key,
                _event: null,
                _actor: null);
            configure(configuration);
            RuleKey key = configuration.Build();
            _condition = key;
            _pluginRules.Add(key);
        });
        return this;
    }

    public IRuleConfiguration<T> Export(Rule<T> instance)
    {
        _lazyConfigurations.Add(() =>
        {
            _services.TryAddKeyedSingleton(_key, instance);
            _services.TryAddSingleton(instance);
        });
        return this;
    }

    public IRuleConfiguration<T> Export<TDefinition>() where TDefinition : class, IRuleDefinition<T>
    {
        _lazyConfigurations.Add(() =>
        {
            _services.TryAddKeyedTransient<TDefinition>(_key);
            _services.TryAddKeyedTransient<Rule<T>>(_key, (sp, key) =>
                sp.GetRequiredKeyedService<TDefinition>(key).Compose(((RuleKey) key).As<T>()));
        });
        return this;
    }
}

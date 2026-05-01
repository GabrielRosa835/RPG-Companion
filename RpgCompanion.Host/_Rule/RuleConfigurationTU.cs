namespace RpgCompanion.Host;

using Core;
using Microsoft.Extensions.DependencyInjection.Extensions;

internal class RuleConfiguration<T, U>(
    IServiceCollection _services,
    PluginKey _plugin,
    ISet<RuleKey> _pluginRules,
    RuleKey? _conditionFor,
    EventKey? _event,
    ActorKey? _actor)
    : IRuleConfiguration<T, U>
{
    private readonly List<Action> _lazyConfigurations = [];
    private RuleKey? _key;
    private RuleKey? _condition;
    private string? _displayName;

    internal RuleKey Build()
    {
        KeyException.ThrowIfNull(_key);
        foreach (var lazyConfiguration in _lazyConfigurations)
        {
            lazyConfiguration();
        }
        _services.AddKeyedSingleton(_key, new RuleDescriptor
        {
            Key = _key.Value,
            Plugin = _plugin,
            Event = _event,
            Actor = _actor,
            ConditionFor = _conditionFor,
            Condition = _condition,
            DisplayName = _displayName,
        });
        return _key.Value;
    }

    public IRuleConfiguration<T, U> WithKey(RuleKey<T, U> key)
    {
        _key = key;
        return this;
    }

    public IRuleConfiguration<T, U> WithName(string name)
    {
        _displayName = name;
        return this;
    }

    public IRuleConfiguration<T, U> WithCondition(Configure<IRuleConfiguration<T, bool>> configure)
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
            var key = configuration.Build();
            _condition = key;
            _pluginRules.Add(key);
        });
        return this;
    }

    public IRuleConfiguration<T, U> Export(Rule<T, U> instance)
    {
        _lazyConfigurations.Add(() =>
        {
            _services.TryAddKeyedSingleton(_key, instance);
            _services.TryAddSingleton(instance);
        });
        return this;
    }

    public IRuleConfiguration<T, U> Export<TDefinition>() where TDefinition : class, IRuleDefinition<T, U>
    {
        _lazyConfigurations.Add(() =>
        {
            _services.AddKeyedTransient<TDefinition>(_key);
            _services.AddKeyedTransient<Rule<T, U>>(_key, (sp, key) =>
                sp.GetRequiredKeyedService<TDefinition>(key).Compose(((RuleKey) key).As<T, U>()));
        });
        return this;
    }
}

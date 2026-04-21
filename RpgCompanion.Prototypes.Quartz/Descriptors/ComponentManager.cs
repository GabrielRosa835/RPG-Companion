namespace RpgCompanion.QuartzPrototype.Descriptors;

// Singleton
public class ComponentManager
{
    private readonly Dictionary<EffectKey, EffectDescriptor> _effects = [];
    private readonly Dictionary<EventKey, EventDescriptor> _events = [];
    private readonly Dictionary<RuleKey, RuleDescriptor> _rules = [];

    internal void AddEffect(EffectDescriptor effect) => _effects[effect.Key] = effect;
    internal EffectDescriptor GetEffect(string key) => _effects[new(key)];

    internal void AddEvent(EventDescriptor eventDescriptor) => _events[eventDescriptor.Key] = eventDescriptor;
    internal EventDescriptor GetEvent(string key) => _events[new(key)];

    internal void AddRule(RuleDescriptor rule) => _rules[rule.Key] = rule;
    internal RuleDescriptor GetRule(string key) => _rules[new(key)];
}

namespace RpgCompanion.Application.Engines;

using RpgCompanion.Application.Services;

internal class RuleCollection
{
    private readonly List<RuleDescriptor> _rules = [];

    internal IEnumerable<RuleDescriptor> Before => _rules
       .Where(d => d.Ordering.HasFlag(RuleOrdering.Before));
    internal IEnumerable<RuleDescriptor> After => _rules
       .Where(d => d.Ordering.HasFlag(RuleOrdering.After));

    internal void SetValues(IEnumerable<RuleDescriptor> rules)
    {
        _rules.Clear();
        _rules.AddRange(rules);
    }
    internal void Clear()
    {
        _rules.Clear();
    }
}

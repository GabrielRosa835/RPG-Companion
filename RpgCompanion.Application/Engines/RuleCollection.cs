using RpgCompanion.Application.Services;

namespace RpgCompanion.Application.Engines;

internal class RuleCollection
{
   internal readonly List<RuleDescriptor> _rules = [];

   internal IEnumerable<RuleDescriptor> AllBefore => _rules
      .Where(d => d.Ordering.HasFlag(RuleOrdering.Before));
   internal IEnumerable<RuleDescriptor> AllAfter => _rules
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

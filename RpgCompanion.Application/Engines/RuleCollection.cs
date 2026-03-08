using RpgCompanion.Application.Services;

namespace RpgCompanion.Application.Engines;

internal class RuleCollection
{
   internal readonly List<ComponentDescriptor> _rules;

   public RuleCollection (IEnumerable<ComponentDescriptor> rules)
   {
      _rules = new(rules);
   }

   internal ComponentDescriptor? FirstOrDefault()
   {
      return _rules.FirstOrDefault();
   }
   internal IEnumerable<ComponentDescriptor> BeforeEvent ()
   {
      return _rules.Where(r => r.Rule_Placement == RulePlacement.BeforeEvent);
   }
   internal IEnumerable<ComponentDescriptor> AfterEvent ()
   {
      return _rules.Where(r => r.Rule_Placement == RulePlacement.AfterEvent);
   }
}

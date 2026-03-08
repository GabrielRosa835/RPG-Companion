using RpgCompanion.Application.Services;

namespace RpgCompanion.Application.Engines;

internal class RuleCollection(IEnumerable<(object Rule, RulePlacement Placement)> values)
{
   internal readonly List<(object Rule, RulePlacement Placement)> _rules = new(values);

   internal IEnumerable<object> BeforeEvent ()
   {
      return WithPlacement(RulePlacement.BeforeEvent);
   }
   internal IEnumerable<object> AfterEvent ()
   {
      return WithPlacement(RulePlacement.AfterEvent);
   }
   //internal IEnumerable<object> BeforeEffect ()
   //{
   //   return WithPlacement(RulePlacement.BeforeEffect);
   //}
   //internal IEnumerable<object> AfterEffect()
   //{
   //   return WithPlacement(RulePlacement.AfterEvent);
   //}

   private IEnumerable<object> WithPlacement(RulePlacement placement)
   {
      return _rules.Where(r => r.Placement == placement).Select(r => r.Rule);
   }
}

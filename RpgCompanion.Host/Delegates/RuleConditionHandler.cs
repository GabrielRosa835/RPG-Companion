namespace RpgCompanion.Host.Delegates;

using Core;
using Core.Engine.Contexts;
using Core.Events;

public class RuleConditionHandler(RuleCondition condition) : IRuleCondition
{
    public bool ShouldApply(IContext e) => condition(e);
}

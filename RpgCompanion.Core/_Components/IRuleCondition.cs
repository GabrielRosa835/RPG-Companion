namespace RpgCompanion.Core;

using Engine.Contexts;

public interface IRuleCondition
{
    public bool ShouldApply(IContext e);
}

public delegate bool RuleCondition(IContext context);

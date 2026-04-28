namespace RpgCompanion.Core;

public interface ICondition<in T>
{
    bool ShouldApply(T current);
}

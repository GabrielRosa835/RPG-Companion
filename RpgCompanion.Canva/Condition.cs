namespace RpgCompanion.Canva;

public interface ICondition<in T>
{
    bool ShouldApply(T current);
}

public delegate bool Condition<in T>(T current);

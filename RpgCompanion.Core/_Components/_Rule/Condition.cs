namespace RpgCompanion.Core;

public record Condition<T>(Func<T, bool> Delegate) : Rule<T, bool>(Delegate), ICondition<T>;

public interface ICondition<in T> : IRule<T, bool>;

public static class Condition
{
    public static ICondition<T> Of<T>(Func<T, bool> condition) => new Condition<T>(condition);
}

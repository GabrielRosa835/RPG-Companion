namespace RpgCompanion.Core;

public record Action<T>(Func<T, IEvent> Delegate) : Rule<T, IEvent>(Delegate), IAction<T>;

public interface IAction<in T> : IRule<T, IEvent>;

public static class Action
{
    public static IAction<T> Of<T>(Func<T, IEvent> action) => new Action<T>(action);
}

namespace RpgCompanion.Core;

public interface IAction<in T> : IRule<T, IEvent>
{
    public static IAction<T> Of(Func<T, IEvent> action) => new ActionWrapper<T>(action);
}

internal readonly record struct ActionWrapper<T>(Func<T, IEvent> Delegate) : IAction<T>
{
    public IEvent Apply(T target) => Delegate.Invoke(target);
}

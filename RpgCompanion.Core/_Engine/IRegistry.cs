namespace RpgCompanion.Core;

public interface IRegistry
{
    public T? Get<T>() where T : class;
    public T GetRequired<T>() where T : class;

    public TActor Get<TActor>(ActorKey<TActor> key) where TActor : class, IActor;
    public TEvent Get<TEvent>(EventKey<TEvent> key) where TEvent : class, IEvent;
    public Rule<T> Get<T>(RuleKey<T> key);
    public Rule<T, U> Get<T, U>(RuleKey<T, U> key);
}

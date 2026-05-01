namespace RpgCompanion.Core;

public interface IRegistry
{
    public T? Get<T>() where T : class;
    public T GetRequired<T>() where T : class;

    public TActor GetActor<TActor>(ActorKey key) where TActor : class, IActor;
    public TActor GetActor<TActor>(ActorKey<TActor> key) where TActor : class, IActor;

    public TEvent GetEvent<TEvent>(EventKey key) where TEvent : class, IEvent;
    public TEvent GetEvent<TEvent>(EventKey<TEvent> key) where TEvent : class, IEvent;

    public Rule<T> GetRule<T>(RuleKey key);
    public Rule<T> GetRule<T>(RuleKey<T> key);

    public Rule<T, U> GetRule<T, U>(RuleKey key);
    public Rule<T, U> GetRule<T, U>(RuleKey<T, U> key);
}

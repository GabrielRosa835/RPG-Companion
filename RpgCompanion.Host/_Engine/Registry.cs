namespace RpgCompanion.Host;

using Core;

public class Registry(IServiceProvider _provider) : IRegistry
{
    public T? Get<T>() where T : class => _provider.GetService<T>();
    public T GetRequired<T>() where T : class => _provider.GetRequiredService<T>();

    public TActor Get<TActor>(ActorKey<TActor> key) where TActor : class, IActor =>
        _provider.GetRequiredKeyedService<TActor>(key);

    public TEvent Get<TEvent>(EventKey<TEvent> key) where TEvent : class, IEvent =>
        _provider.GetRequiredKeyedService<TEvent>(key);

    public IRule<T> Get<T>(RuleKey<T> key) => _provider.GetRequiredKeyedService<IRule<T>>(key);

    public IRule<T, U> Get<T, U>(RuleKey<T, U> key) => _provider.GetRequiredKeyedService<IRule<T, U>>(key);
}

public static class ServiceProviderExtensions
{
    public static IRegistry AsRegistry(this IServiceProvider provider)
    {
        return new Registry(provider);
    }
}

namespace RpgCompanion.Host;

using System.Collections.Concurrent;

public class Mediator(IServiceProvider _serviceProvider)
{
    private readonly ConcurrentDictionary<Type, Type> _handlers = new();

    public void Register<TIntent, TResult, TIntentHandler>()
        where TIntent : IIntent<TResult>
        where TIntentHandler : IIntentHandler<TIntent, TResult>
    {
        _handlers.TryAdd(typeof(TIntent), typeof(TIntentHandler));
    }

    public void Register<TIntent, TIntentHandler>()
        where TIntent : IIntent
        where TIntentHandler : IIntentHandler<TIntent>
    {
        _handlers.TryAdd(typeof(TIntent), typeof(TIntentHandler));
    }

    public Task<TResult> Send<TIntent, TResult>(TIntent intent) where TIntent : IIntent<TResult>
    {
        IIntentHandler<TIntent, TResult> handler = (IIntentHandler<TIntent, TResult>)
            _serviceProvider.GetRequiredService(_handlers[typeof(TIntent)]);
        return handler.Handle(intent);
    }

    public Task Send<TIntent>(TIntent intent) where TIntent : IIntent
    {
        IIntentHandler<TIntent> handler = (IIntentHandler<TIntent>)
            _serviceProvider.GetRequiredService(_handlers[typeof(TIntent)]);
        return handler.Handle(intent);
    }
}

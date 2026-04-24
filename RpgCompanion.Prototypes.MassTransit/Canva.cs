namespace RpgCompanion.Prototypes.MassTransit;

using Core;
using global::MassTransit;

public class EventPublisher
{
    public void Canva(IBus bus)
    {
        bus.Publish(new EmptyEvent(), context =>
        {
            context.Headers.Set("eventKey", "aoefnawefnaw", overwrite: true);
        });
    }
}

public class EffectCondition<TEvent> : IConsumeObserver where TEvent : IEvent
{
    public Task PreConsume<T>(ConsumeContext<T> context) where T : class
    {
        if (!context.TryGetPayload(out IServiceProvider provider))
        {
            throw new InvalidOperationException();
        }
        context.Headers.
        var condition =  provider.GetService<TEvent>();
    }

    public Task PostConsume<T>(ConsumeContext<T> context) where T : class
    {
        throw new NotImplementedException();
    }

    public Task ConsumeFault<T>(ConsumeContext<T> context, Exception exception) where T : class
    {
        throw new NotImplementedException();
    }
}

public class EffectConsumer<TEvent>(IEffect<TEvent>? effect, IAsyncEffect<TEvent>? asyncEffect) : IConsumer<TEvent> where TEvent : class, IEvent
{
    public Task Consume(ConsumeContext<TEvent> context)
    {
        effect?.Apply(context.Message);
        var task = asyncEffect?.ApplyAsync(context.Message);
        return task ?? Task.CompletedTask;
    }
}

namespace RpgCompanion.Host;

using Core;
using MediatR;

public interface IInternalEventHandler
{
    Task HandleAsync(EventRaisedEvent e, CancellationToken cancellationToken);
}

public class EventRaisedRouter(IServiceProvider serviceProvider) : INotificationHandler<EventRaisedEvent>
{
    public Task Handle(EventRaisedEvent message, CancellationToken cancellationToken)
    {
        var eventType = message.Event.GetType();
        var handlerType = typeof(EventRaisedHandler<>).MakeGenericType(eventType);
        var handler = (IInternalEventHandler)ActivatorUtilities.CreateInstance(serviceProvider, handlerType);
        return handler.HandleAsync(message, cancellationToken);
    }
}

public class EventRaisedHandler<TEvent>(IServiceProvider _serviceProvider, IComponentGraph _components)
    : IInternalEventHandler
    where TEvent : IEvent
{
    public async Task HandleAsync(EventRaisedEvent e, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var currentState = (TEvent) e.Event;
        var pipelineSteps = new List<(double Order, Func<TEvent, Task<TEvent>> Execute)>();

        var descriptor = _components.Events.FirstOrDefault(d => d.Type == typeof(TEvent))
            ?? throw new InvalidOperationException($"Could not find a descriptor for event of type {typeof(TEvent)}");

        var ruleKeys = descriptor.Connections.Rules;
        var actionKeys = descriptor.Connections.Actions;

        foreach (var ruleKey in ruleKeys)
        {
            var ruleDescriptor = serviceProvider.GetRequiredKeyedService<RuleDescriptor>(ruleKey);
            var ruleDelegate = serviceProvider.GetRequiredKeyedService<Rule<TEvent>>(ruleKey);
            pipelineSteps.Add((ruleDescriptor.Order, state =>
            {
                if (ruleDescriptor.Connections.Conditions.All(c =>
                    {
                        var conditionDelegate = serviceProvider.GetKeyedService<Rule<TEvent, bool>>(c);
                        return conditionDelegate == null || conditionDelegate(state);
                    }))
                {
                    state = ruleDelegate(state);
                }
                return Task.FromResult(state);
            }));
        }

        foreach (var actionKey in actionKeys)
        {
            var ruleDescriptor = serviceProvider.GetRequiredKeyedService<RuleDescriptor>(actionKey);
            var ruleDelegate = serviceProvider.GetRequiredKeyedService<Rule<TEvent, IEvent>>(actionKey);
            pipelineSteps.Add((ruleDescriptor.Order, async state =>
            {
                if (ruleDescriptor.Connections.Conditions.All(c =>
                    {
                        var conditionDelegate = serviceProvider.GetKeyedService<Rule<TEvent, bool>>(c);
                        return conditionDelegate == null || conditionDelegate(state);
                    }))
                {
                    var generatedEvent = ruleDelegate(state);
                    if (generatedEvent != null)
                    {
                        await mediator.Publish(generatedEvent, cancellationToken);
                    }
                }
                return state;
            }));
        }

        foreach (var step in pipelineSteps.OrderBy(x => x.Order))
        {
            currentState = await step.Execute(currentState);
        }

        if (!e.Transitions.TryDequeue(out var nextTransition))
        {
            return;
        }

        IEvent nextEvent = nextTransition(currentState);

        var nextEventType = nextEvent.GetType();
        var nextDescriptor = _components.Events.FirstOrDefault(d => d.Type == nextEventType)
            ?? throw new InvalidOperationException($"Could not find a descriptor for event of type {nextEventType}");

        var nextMessage = new EventRaisedEvent
        {
            Event = nextEvent,
            Descriptor = nextDescriptor,
            Transitions = e.Transitions
        };

        await mediator.Publish(nextMessage, cancellationToken);
    }
}

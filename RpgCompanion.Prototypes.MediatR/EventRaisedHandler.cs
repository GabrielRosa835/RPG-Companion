namespace RpgCompanion.Prototypes.MediatR;

using Core;
using global::MediatR;

public class EventRaisedHandler<TEvent>(IServiceProvider _serviceProvider, IComponentGraph _components)
    : INotificationHandler<EventRaisedEvent<TEvent>>
    where TEvent : IEvent
{
    public async Task Handle(EventRaisedEvent<TEvent> message, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var currentState = message.Event;
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
                        // MediatR can publish raw objects directly
                        await mediator.Publish((object)generatedEvent, cancellationToken);
                    }
                }
                return state;
            }));
        }

        foreach (var step in pipelineSteps.OrderBy(x => x.Order))
        {
            currentState = await step.Execute(currentState);
        }

        if (!message.Transitions.TryDequeue(out var nextTransition))
        {
            return;
        }

        var transitionRule = _serviceProvider.GetRequiredKeyedService<Rule<TEvent, IEvent>>(nextTransition);
        IEvent nextEvent = transitionRule(currentState);

        var nextEventType = nextEvent.GetType();
        var wrapperType = typeof(EventRaisedEvent<>).MakeGenericType(nextEventType);
        var nextDescriptor = _components.Events.FirstOrDefault(d => d.Type == nextEventType)
            ?? throw new InvalidOperationException($"Could not find a descriptor for event of type {nextEventType}");

        var nextMessage = Activator.CreateInstance(wrapperType, nextEvent, nextDescriptor, message.Transitions);

        // MediatR naturally resolves the runtime type when cast to object
        await mediator.Publish((object)nextMessage!, cancellationToken);
    }
}

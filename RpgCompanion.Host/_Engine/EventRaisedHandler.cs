namespace RpgCompanion.Host;

using System.Text.Json;
using MassTransit;
using RpgCompanion.Core;

internal class EventRaisedHandler<TEvent>(IServiceScopeFactory _scopeFactory, IComponentGraph _components)
    where TEvent : IEvent
{
    public async Task Handle(TEvent e, IEnumerable<Transition> transitions, IPublishEndpoint publisher, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        var pipelineSteps = new List<(double Order, Func<TEvent, Task<TEvent>> Execute)>();

        var descriptor = _components.Events.Find<TEvent>();
        var ruleKeys = descriptor.Connections.Rules;
        var actionKeys = descriptor.Connections.Actions;

        foreach (var ruleKey in ruleKeys)
        {
            var ruleDescriptor = serviceProvider.GetRequiredKeyedService<RuleDescriptor>(ruleKey);
            var rule = serviceProvider.GetRequiredKeyedService<IRule<TEvent>>(ruleKey);
            pipelineSteps.Add((ruleDescriptor.Order, EffectExecutor(ruleDescriptor, rule, serviceProvider)));
        }

        foreach (var actionKey in actionKeys)
        {
            var ruleDescriptor = serviceProvider.GetRequiredKeyedService<RuleDescriptor>(actionKey);
            var rule = serviceProvider.GetRequiredKeyedService<IRule<TEvent, IEvent>>(actionKey);
            Task Publisher(IEvent e) => publisher.Publish(e, cancellationToken);
            pipelineSteps.Add((ruleDescriptor.Order, ActionExecutor(ruleDescriptor, rule, serviceProvider, Publisher)));
        }

        foreach (var step in pipelineSteps.OrderBy(x => x.Order))
        {
            e = await step.Execute(e);
        }

        foreach (Transition transition in transitions)
        {
            var transitionRule = serviceProvider.GetRequiredKeyedService<IRule<TEvent, IEvent>>(transition.Key);

            IEvent nextEvent = transitionRule.Apply(e);

            var nextEventType = nextEvent.GetType();
            var nextDescriptor = _components.Events.Find(nextEventType);

            var nextMessage = new EventRaisedEvent
            {
                Data = JsonSerializer.SerializeToElement(nextEvent, nextEventType),
                Key = nextDescriptor.Key,
                Transitions = transition.Chain,
            };

            await publisher.Publish(nextMessage, cancellationToken);
        }
    }

    private static Func<TEvent, Task<TEvent>> EffectExecutor(
        RuleDescriptor effectDescriptor,
        IRule<TEvent> effect,
        IServiceProvider serviceProvider) => state =>
    {
        bool CheckCondition(RuleKey c)
        {
            var conditionDelegate = serviceProvider.GetKeyedService<IRule<TEvent, bool>>(c);
            return conditionDelegate == null || conditionDelegate.Apply(state);
        }
        if (effectDescriptor.Connections.Conditions.All(CheckCondition))
        {
            state = effect.Apply(state);
        }
        return Task.FromResult(state);
    };

    private static Func<TEvent, Task<TEvent>> ActionExecutor(
        RuleDescriptor actionDescriptor,
        IRule<TEvent, IEvent> action,
        IServiceProvider serviceProvider,
        Func<IEvent, Task> publisher) => async state =>
    {
        bool CheckCondition(RuleKey c)
        {
            var conditionDelegate = serviceProvider.GetKeyedService<IRule<TEvent, bool>>(c);
            return conditionDelegate == null || conditionDelegate.Apply(state);
        }
        if (actionDescriptor.Connections.Conditions.All(CheckCondition))
        {
            var generatedEvent = action.Apply(state);
            if (generatedEvent != null)
            {
                await publisher(generatedEvent);
            }
        }
        return state;
    };
}

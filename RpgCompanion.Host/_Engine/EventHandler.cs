using RpgCompanion.Core;

namespace RpgCompanion.Host;

internal abstract class EventHandler
{
    public abstract void Handle(EventContext context);

    public static EventHandler CreateTyped(Type eventType, IServiceProvider serviceProvider)
    {
        var handlerType = typeof(TypedEventHandler<>).MakeGenericType(eventType);
        return (EventHandler) ActivatorUtilities.CreateInstance(serviceProvider, handlerType);
    }
}

internal class TypedEventHandler<TEvent>(
    IServiceScopeFactory _scopeFactory,
    IComponentGraph _components,
    IEventPublisher _publisher,
    ITrigger _trigger)
    : EventHandler where TEvent : IEvent
{
    private RuleContext _context = default!;
    private IServiceProvider _serviceProvider = default!;

    public override void Handle(EventContext context)
    {
        using var scope = _scopeFactory.CreateScope();

        _serviceProvider = scope.ServiceProvider;
        _context = _serviceProvider.GetRequiredService<RuleContext>();

        var pipelineSteps = new List<(double Order, Func<TEvent, TEvent> Execute)>();

        TEvent e = (TEvent) context.Data;
        var ruleKeys = context.Descriptor.Rules;
        var actionKeys = context.Descriptor.Actions;

        foreach (var ruleKey in ruleKeys)
        {
            var ruleDescriptor = _serviceProvider.GetRequiredKeyedService<RuleDescriptor>(ruleKey);
            var rule = _serviceProvider.GetRequiredKeyedService<Rule<TEvent>>(ruleKey);
            pipelineSteps.Add((ruleDescriptor.Order, EffectExecutor(ruleDescriptor, rule)));
        }

        foreach (var actionKey in actionKeys)
        {
            var ruleDescriptor = _serviceProvider.GetRequiredKeyedService<RuleDescriptor>(actionKey);
            var rule = _serviceProvider.GetRequiredKeyedService<Rule<TEvent, IEvent>>(actionKey);
            pipelineSteps.Add((ruleDescriptor.Order, ActionExecutor(ruleDescriptor, rule)));
        }

        foreach (var step in pipelineSteps.OrderBy(x => x.Order))
        {
            e = step.Execute.Invoke(e);
        }

        foreach (EventTransition transition in context.Transitions)
        {
            IEvent nextEvent = transition.Rule.Invoke(e, _context);

            var nextEventType = nextEvent.GetType();
            var nextDescriptor = _components.Events.Find(nextEventType);

            var nextMessage = new EventContext
            {
                Data = nextEvent,
                Descriptor = nextDescriptor,
                Transitions = transition.Chain,
            };

            _publisher.Publish(nextMessage);
        }
    }

    private Func<TEvent, TEvent> EffectExecutor(
        RuleDescriptor effectDescriptor,
        Rule<TEvent> effect) => state =>
    {
        if (effectDescriptor.Conditions.All(CheckCondition(state)))
        {
            state = effect.Invoke(state, _context);
        }
        return state;
    };

    private Func<TEvent, TEvent> ActionExecutor(
        RuleDescriptor actionDescriptor,
        Rule<TEvent, IEvent> action) => state =>
    {
        if (actionDescriptor.Conditions.All(CheckCondition(state)))
        {
            var generatedEvent = action.Invoke(state, _context);
            if (generatedEvent != null)
            {
                _trigger.Raise(generatedEvent);
            }
        }
        return state;
    };

    private Func<RuleKey, bool> CheckCondition(TEvent state) => key =>
    {
        var conditionDelegate = _serviceProvider.GetKeyedService<Rule<TEvent, bool>>(key);
        return conditionDelegate == null || conditionDelegate.Invoke(state, _context);
    };
}

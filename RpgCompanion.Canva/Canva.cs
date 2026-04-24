namespace RpgCompanion.Canva;

using Microsoft.Extensions.DependencyInjection;

public interface IEvent;

public static class DiceRoll
{
    public static Rule<Event> RollDice => e =>
    {
        e.Result = e.Dice.Roll();
        return e;
    };
    public class Event
    {
        public Dice Dice { get; init; }
        public int Result { get; set; }
    }
}

public record EventHandling<TEvent>(int Order, string? ruleKey, string? effectKey);

public class EventHandler<TEvent>(TEvent e, IEnumerable<EventHandling<TEvent>> handlers, IServiceProvider serviceProvider)
{
    private TEvent _event = e;

    public void Handle()
    {
        foreach (var handler in handlers.OrderBy(h => h.Order))
        {
            if (handler.ruleKey is not null)
            {
                var rule = serviceProvider.GetRequiredKeyedService<Rule<TEvent>>(handler.ruleKey);
                _event = rule.Invoke(_event);
            }
            else if (handler.effectKey is not null)
            {
                var effect = serviceProvider.GetRequiredKeyedService<Effect<TEvent, IEvent>>(handler.effectKey);
                var newEvent = effect.Invoke(_event);
                // Queue newEvent
            }
        }
    }
}

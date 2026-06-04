namespace RpgCompanion.DnD;

using Core;
using Utils.Storage;

public static class DiceRoll
{
    public static Key<int> Result {get;} = typeof(Event).FullName! + "-Result";

    public record Event(Dice.D6 Dice, int Modifier) : IEvent
    {
        public static EventKey<Event> Key { get; } = typeof(Event).FullName!;
    }

    public class Rule(ContextData context) : IRule<Event>
    {
        public static RuleKey<Event> Key { get; } = typeof(Rule).FullName!;

        public Event Apply(Event target)
        {
            Console.WriteLine($"""
                               Realizando efeito de rolagem:
                               Dado: {target.Dice}
                               """);
            var result = target.Dice.Roll() + target.Modifier;

            context.Add(Result, result);

            Console.WriteLine($"Rolagem realizada com resultado: {result}");
            return target;
        }
    }
}

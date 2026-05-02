namespace RpgCompanion.DnD;

using Core;

public static class DiceRoll
{
    public record Event(Dice Dice, int Modifier) : IEvent
    {
        public static EventKey<Event> Key = nameof(DiceRoll);
        public int Result { get; set; }
    }

    public static class Apply
    {
        public static RuleKey<Event> Key = typeof(Apply).FullName!;
        public static Rule<Event> Rule = e =>
        {
            Console.WriteLine($"""
               Realizando efeito de rolagem:
               Dado: {e.Dice}
               """);
            e.Result = e.Dice.Roll() + e.Modifier;
            Console.WriteLine($"Rolagem realizada com resultado: {e.Result}");
            return e;
        };
    }
}

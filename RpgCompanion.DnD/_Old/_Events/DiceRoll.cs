namespace RpgCompanion.DnD._Old._Events;

using RpgCompanion.Events;
using RpgCompanion.Toolbox;

public static class DiceRoll
{
    public static StorageKey<int> Result { get; } = typeof(Event).FullName! + "-Result";

    public record Event(Dice.D6 Dice, int Modifier) : IEvent
    {
        public static EventKey<Event> Key { get; } = typeof(Event).FullName!;
    }

    public static Event Handler(Event e, RuleContext ctx)
    {
        Console.WriteLine($"""
                           Realizando efeito de rolagem:
                           Dado: {e.Dice}
                           """);
        var result = e.Dice.Roll() + e.Modifier;

        ctx.Put(Result, result);

        Console.WriteLine($"Rolagem realizada com resultado: {result}");
        return e;
    }
}

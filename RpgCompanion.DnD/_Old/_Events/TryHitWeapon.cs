namespace RpgCompanion.DnD._Old._Events;

using _Actors;
using RpgCompanion.Events;

public static class TryHitWeapon
{
    public record Event(int AttackResult, Enemy Defender) : IEvent
    {
        public static EventKey<Event> Key { get; } = typeof(Event).FullName!;
        public bool Hit { get; set; }
    }

    public static Event Handler(Event e, RuleContext ctx)
    {
        Console.WriteLine($"""
                           Realizando efeito de tentativa de ataque
                           Target: {e.Defender.AC}
                           Modifier: {e.AttackResult}
                           """);

        var hit = e.AttackResult >= e.Defender.AC;
        e.Hit = hit;

        Console.WriteLine($"Resultado: {e.AttackResult}");
        Console.WriteLine($"Sucesso: {hit}");

        return e;
    }
}

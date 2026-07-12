namespace RpgCompanion.DnD._Old._Events;

using _Actors;
using RpgCompanion.Events;

public static class DealDamage
{
    public record Event(Enemy Enemy, int Damage) : IEvent
    {
        public static EventKey<Event> Key { get; } = typeof(Event).FullName!;
    }

    public static bool ShouldApply(Event e, RuleContext ctx)
    {
        return e.Damage > 0;
    }

    public static Event Handler(Event e, RuleContext ctx)
    {
        Console.WriteLine($"""
                           Realizando efeito de dano
                           Defensor: {e.Enemy.Name} ({e.Enemy.Health}HP)
                           Dano: {e.Damage}
                           """);

        e.Enemy.Health -= e.Damage;
        if (e.Enemy.Health <= 0)
        {
            e.Enemy.Health = 0;
        }

        Console.WriteLine($"Vida após dano: {e.Enemy.Health}HP");
        return e;
    }
}

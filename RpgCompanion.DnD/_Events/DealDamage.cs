namespace RpgCompanion.DnD;

using Core;

public static class DealDamage
{
    public record Event(Enemy Enemy, int Damage) : IEvent
    {
        public static EventKey<Event> Key { get; } = typeof(Event).FullName!;
    }

    public class ShouldApply : ICondition<Event>
    {
        public static RuleKey<Event, bool> Key { get; } = typeof(ShouldApply).FullName!;
        public bool Apply(Event e) => e.Damage > 0;
    }

    public class Rule : IRule<Event>
    {
        public static RuleKey<Event> Key { get; } = typeof(Rule).FullName!;

        public Event Apply(Event target)
        {
            Console.WriteLine($"""
                               Realizando efeito de dano
                               Defensor: {target.Enemy.Name} ({target.Enemy.Health}HP)
                               Dano: {target.Damage}
                               """);

            target.Enemy.Health -= target.Damage;
            if (target.Enemy.Health <= 0)
            {
                target.Enemy.Health = 0;
            }

            Console.WriteLine($"Vida após dano: {target.Enemy.Health}HP");
            return target;
        }
    }
}

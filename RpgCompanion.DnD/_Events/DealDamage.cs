namespace RpgCompanion.DnD;

using Core;

public static class DealDamage
{
    public record Event(Defender Defender, int Damage) : IEvent
    {
        public static EventKey<Event> Key = typeof(Event).FullName!;
    }

    public static class ShouldApply
    {
        public static RuleKey<Event, bool> Key = typeof(ShouldApply).FullName!;
        public static Rule<Event, bool> Rule = (e) => e.Damage > 0;
    }

    public static class Apply
    {
        public static RuleKey<Event> Key = typeof(Apply).FullName!;
        public static Rule<Event> Rule => (e) =>
        {
            Console.WriteLine($"""
                Realizando efeito de dano
                Defensor: {e.Defender.Name} ({e.Defender.Health}HP)
                Dano: {e.Damage}
                """);

            e.Defender.Health -= e.Damage;
            if (e.Defender.Health <= 0)
            {
                e.Defender.Health = 0;
            }

            Console.WriteLine($"Vida após dano: {e.Defender.Health}HP");
            return e;
        };
    }
}

namespace RpgCompanion.DnD;

using Core;

public static class TryHitWeapon
{
    public record Event(int AttackResult, Enemy Defender) : IEvent
    {
        public static EventKey<Event> Key { get; } = typeof(Event).FullName!;
        public bool Hit { get; set; }
    }

    public class Rule : IRule<Event>
    {
        public static RuleKey<Event> Key { get; } = typeof(Rule).FullName!;

        public Event Apply(Event target)
        {
            Console.WriteLine($"""
                               Realizando efeito de tentativa de ataque
                               Target: {target.Defender.AC}
                               Modifier: {target.AttackResult}
                               """);

            var hit = target.AttackResult >= target.Defender.AC;
            target.Hit = hit;

            Console.WriteLine($"Resultado: {target.AttackResult}");
            Console.WriteLine($"Sucesso: {hit}");

            return target;
        }
    }
}

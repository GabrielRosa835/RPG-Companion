namespace RpgCompanion.DnD;

using Core;

public static class TryHitWeapon
{
    public record Event(int Modifier, int TargetValue) : IEvent
    {
        public static EventKey<Event> Key = typeof(Event).FullName!;
        public bool Hit { get; set; }
    }
    public static class Apply
    {
        public static RuleKey<Event> Key = typeof(Apply).FullName!;
        public static Rule<Event> Rule => (e) =>
        {
            Console.WriteLine($"""
               Realizando efeito de tentativa de ataque
               Target: {e.TargetValue}
               Modifier: {e.Modifier}
               """);

            int result = new Dice.D20().Roll() + e.Modifier;
            var hit = result >= e.TargetValue;
            e.Hit = hit;

            Console.WriteLine($"Resultado: {result}");
            Console.WriteLine($"Sucesso: {hit}");

            return e;
        };
    }
}

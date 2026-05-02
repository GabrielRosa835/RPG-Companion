namespace RpgCompanion.DnD;

using Core;

public static class Attack
{
    public record Event(Attacker Attacker, Defender Defender) : IEvent
    {
        public static EventKey<Event> Key = typeof(Event).FullName!;
    }

    public class Definition(ITrigger trigger) : IRuleDefinition<Event>
    {
        public static RuleKey<Event> Key = typeof(Definition).FullName!;
        public Rule<Event> Compose() => e =>
        {
            Console.WriteLine($"""
                Realizando efeito de ataque:
                Atacante: {e.Attacker.Name}
                Defensor: {e.Defender.Name}
                """);
            var diceRoll = new DiceRoll.Event(e.Attacker.Weapon!.DamageDice, e.Attacker.AttackModifier);

            trigger.Raise(diceRoll, pipeline => pipeline
                .Then(roll => new DealDamage.Event(e.Defender, roll.Result)));

            return e;
        };
    }
}

namespace RpgCompanion.DnD;

using System.Formats.Asn1;
using Core;
using Core.Toolbox;

public static class Attack
{
    public static StorageKey<Enemy> DefenderKey { get; } = $"{typeof(Attack).FullName!}-Defender";

    public record Event(Player Attacker, Enemy Defender) : IEvent
    {
        public static EventKey<Event> Key { get; } = typeof(Event).FullName!;
    }

    public class DiceRollTransition(GlobalData global, ContextData context) : IRule<DiceRoll.Event, IEvent>
    {
        public static RuleKey<DiceRoll.Event, IEvent> Key { get; } = typeof(DiceRollTransition).FullName!;

        public IEvent Apply(DiceRoll.Event target)
        {
            var defender = global.Get(DefenderKey);
            var result = context.Get(DiceRoll.Result);
            global.Remove(DefenderKey);
            return new DealDamage.Event(defender, result);
        }
    }

    public class Rule(GlobalData context, ITrigger trigger) : IRule<Event>
    {
        public static RuleKey<Event> Key { get; } = typeof(Rule).FullName!;

        public Event Apply(Event target)
        {
            Console.WriteLine($"""
                               Realizando efeito de ataque:
                               Atacante: {target.Attacker.Name}
                               Defensor: {target.Defender.Name}
                               """);

            context.Add(DefenderKey, target.Defender);
            var diceRoll = new DiceRoll.Event(target.Attacker.Weapon!.DamageDice, target.Attacker.AttackModifier);

            trigger.Raise(diceRoll, p => p
                .Then(DiceRollTransition.Key));

            return target;
        }
    }
}

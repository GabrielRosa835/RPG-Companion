namespace RpgCompanion.DnD;

using Core;

public record Attack([Key(Attack.Key)] Attacker Attacker, Defender Defender) : IEvent
{
    public const string Key = nameof(Attack);

    public static Rule<Attack> DoAttack(ITrigger trigger) => (Attack e) =>
    {
        Console.WriteLine(
            $"Realizando efeito de ataque: \n   Atacante: {e.Attacker.Name} \n   Defensor: {e.Defender.Name}");
        trigger
            .Raise(new DiceRoll(e.Attacker.Weapon!.DamageDice, e.Attacker.AttackModifier))
            .FollowedBy(roll => new DealDamage(e.Defender, roll.Result));
        return e;
    };
    public class Definition(ITrigger trigger) : IRuleDefinition<Attack>
    {
        public Rule<Attack> Compose(RuleKey<Attack> key) => DoAttack(trigger);
    }
}

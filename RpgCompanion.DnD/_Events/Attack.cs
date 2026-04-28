namespace RpgCompanion.DnD;

using Core;

public record Attack(Attacker Attacker, Defender Defender) : IEvent
{
    public static Rule<Attack> Apply(IPipeline pipeline) => (e) =>
    {
        Console.WriteLine(
            $"Realizando efeito de ataque: \n   Atacante: {e.Attacker.Name} \n   Defensor: {e.Defender.Name}");
        pipeline
            .Raise(new DiceRoll(e.Attacker.Weapon!.DamageDice, e.Attacker.AttackModifier))
            .FollowedBy(roll => new DealDamage(e.Defender, roll.Result));
        return e;
    };
}

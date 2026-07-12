namespace RpgCompanion.DnD._Old._Events;

using _Actors;
using RpgCompanion.Events;
using RpgCompanion.Toolbox;
using Player = _Actors.Player;

public static class Attack
{
    public static StorageKey<Enemy> DefenderKey = "Defender";

    public record Event(Player Attacker, Enemy Defender) : IEvent;

    public static IEvent Transition(DiceRoll.Event e, RuleContext ctx)
    {
        var defender = ctx.Get(DefenderKey);
        var result = ctx.Get(DiceRoll.Result);
        return new DealDamage.Event(defender, result);
    }

    public static Event Handler(Event e, RuleContext ctx)
    {
        Console.WriteLine($"""
            Realizando efeito de ataque:
            Atacante: {e.Attacker.Name}
            Defensor: {e.Defender.Name}
            """);

        ctx.Put(DefenderKey, e.Defender);
        var diceRoll = new DiceRoll.Event(e.Attacker.Weapon!.DamageDice, e.Attacker.AttackModifier);

        ctx.Raise(diceRoll, p => p.Then(Transition));

        return e;
    }
}

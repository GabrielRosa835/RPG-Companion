namespace RpgCompanion.DnD._Old._Actors;

using _Events;
using RpgCompanion.Events;
using RpgCompanion.Toolbox;

public class Player
{
    public Weapon? Weapon { get; set; }
    public int AttackModifier { get; set; }
    public int DamageModifier { get; set; }
    public string Name { get; set; } = default!;

    public static Attack.Event Attack(Player player, RuleContext ctx)
    {
        StorageKey<Enemy> defenderKey = "key";
        var defender = ctx.Get(defenderKey);
        return new Attack.Event(player, defender);
    }
}

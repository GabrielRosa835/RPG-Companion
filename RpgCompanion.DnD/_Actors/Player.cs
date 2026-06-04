namespace RpgCompanion.DnD;

using Core;

public class Player
{
    public Weapon? Weapon { get; set; }
    public int AttackModifier { get; set; }
    public int DamageModifier { get; set; }
    public string Name { get; set; } = default!;

    public class Attack(Enemy defender) : IRule<Player, DnD.Attack.Event>
    {
        public DnD.Attack.Event Apply(Player target) => new(target, defender);
    }
}

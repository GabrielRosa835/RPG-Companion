namespace RpgCompanion.DnD.Canva;

public record Weapon (Dice DamageDice);
public record Attacker (Weapon CurrentWeapon);

public class Defender (int Health, bool IsDead)
{
   public int Health { get; set; } = Health;
   public int MaxHealth { get; set; } = Health;
   public bool IsDead { get; set; } = IsDead;
}
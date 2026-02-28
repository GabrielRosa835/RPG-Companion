using RpgCompanion.Canva;

namespace RpgCompanion.DnD;

public class Weapon : IObject
{
   public int Damage { get; set; }
   public void Accept (IEffect effect, IContext context)
   {
      effect.Apply(this, context);
   }
}

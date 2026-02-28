namespace RpgCompanion.Canva;

public interface IObject
{
   void Accept (IEffect effect, IContext context);
}

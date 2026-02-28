namespace RpgCompanion.Canva;

public abstract class Action : IAction
{
   public void Apply (IContext context)
   {
      For(null!, context);
   }

   public abstract void For (IActor actor, IContext context);
}

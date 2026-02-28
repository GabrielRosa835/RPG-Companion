using RpgCompanion.Canva;

namespace RpgCompanion.DnD;

public static class Delegates
{
   public static ITarget ToTarget(this Func<IContext, IObject> @delegate)
   {
      return new TargetDelegate(@delegate);
   }
}
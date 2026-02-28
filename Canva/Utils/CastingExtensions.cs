namespace RpgCompanion.Canva.Utils;

public static class CastingExtensions
{
   public static T As<T>(this object obj)
   {
      return (T)obj;
   }
   public static T As<T, U>(this U obj) where U : T
   {
      return obj;
   }
}

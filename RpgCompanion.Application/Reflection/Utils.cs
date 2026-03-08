namespace RpgCompanion.Application.Reflection;

internal static class Utils
{
   public static T As<T> (this object obj)
   {
      return (T) obj;
   }
}

namespace RpgCompanion.Application.Reflection;

internal static class Utils
{
   public static T As<T> (this object obj)
   {
      return (T) obj;
   }

   public static bool Implements(this Type type, Type interfaceType)
   {
      return !(type.IsInterface || type.IsAbstract) && type.GetInterfaces().Contains(interfaceType);
   }
}

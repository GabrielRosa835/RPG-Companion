namespace RpgCompanion.Canva;

public interface IRegistry
{
   void Add<T> ();
   T? Get<T> ();
}

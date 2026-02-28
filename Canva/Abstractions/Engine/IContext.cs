namespace RpgCompanion.Canva;

public interface IContext
{
   IEngine Engine { get; }
   List<IObject> Objects { get; }
}

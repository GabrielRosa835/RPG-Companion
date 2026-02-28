namespace RpgCompanion.Canva;

public interface ITargetGroup
{
   ICollection<IObject> Select(IObject obj);
}

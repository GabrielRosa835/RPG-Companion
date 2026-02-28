namespace RpgCompanion.Canva;

public interface IActor : IObject
{
   void Act(IAction action, IContext context);
}

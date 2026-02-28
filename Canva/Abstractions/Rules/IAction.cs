namespace RpgCompanion.Canva;

public interface IAction : IRule
{
   void For(IActor actor, IContext context);
}

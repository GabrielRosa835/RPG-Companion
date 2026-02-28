namespace RpgCompanion.Canva;

public interface IPrecondition : IRule
{
   void Apply(IEvent @event, IContext context);
}

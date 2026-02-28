namespace RpgCompanion.Canva;

public abstract class PreconditionBase : IPrecondition
{
   public abstract void Apply (IEvent @event, IContext context);

   public void Apply (IContext context)
   {
      Apply(new EmptyEvent(), context);
   }
}

using RpgCompanion.Core.Contexts;

namespace RpgCompanion.Core.Events.Producers;

public interface IRule<in TEvent> where TEvent : IEvent
{
   bool ShouldApply(ISnapshot context);
   IEvent Apply (ISnapshot context);
}

public record EmptyRule<TEvent> : IRule<TEvent> where TEvent : IEvent
{
   public bool ShouldApply(ISnapshot context)
   {
      return true;
   }

   public IEvent Apply(ISnapshot context)
   {
      Console.WriteLine($"{GetType().Name} applied");
      return new EmptyEvent();
   }
}
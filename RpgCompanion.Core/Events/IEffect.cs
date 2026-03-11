using RpgCompanion.Core.Contexts;

namespace RpgCompanion.Core.Events;

public interface IEffect<in TEvent> where TEvent : IEvent
{
   bool ShouldApply(TEvent @event, IContext context);
   void Apply (TEvent @event, IContext context);
}

public record EmptyEffect<TEvent> : IEffect<TEvent> where  TEvent : IEvent
{
   public bool ShouldApply(TEvent @event, IContext context) => true;
   public void Apply(TEvent @event, IContext context)
   {
      Console.WriteLine($"{GetType().Name} applied for event {@event.GetType().Name}");
   }
}
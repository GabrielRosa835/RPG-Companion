using RpgCompanion.Core.Contexts;

namespace RpgCompanion.Core.Events.Producers;

public interface IRule
{
   bool ShouldApply(ISnapshot context);
   IEvent Apply (ISnapshot context);
}

public interface IRule<in TEvent> where TEvent : IEvent
{
   bool ShouldApply(ISnapshot context);
   IEvent Apply (ISnapshot context);
}
namespace RpgCompanion.Core.Events.Producers;

using RpgCompanion.Core.Contexts;

public interface IRule<in TEvent> where TEvent : IEvent
{
    public bool ShouldApply(IContext context);
    public IEvent Apply(IContext context);
}

using RpgCompanion.Core.Events;

namespace RpgCompanion.Core.Engine;

public interface IRegistry
{
   public IEffect<TEvent>? GetEffectFor<TEvent>() where TEvent : IEvent;
}
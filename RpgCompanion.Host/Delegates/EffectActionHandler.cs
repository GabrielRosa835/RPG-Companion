namespace RpgCompanion.Host.Delegates;

using Core;
using Core.Events;

public class EffectActionHandler<TEvent>(EffectAction<TEvent> action) : IEffect<TEvent> where TEvent : IEvent
{
    public void Apply(TEvent e) => action(e, default!);
}

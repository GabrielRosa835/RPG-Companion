namespace RpgCompanion.Core;

using Engine;

public interface IEffect<TEvent> where TEvent : IEvent
{
    public void Apply(TEvent e);
}

public delegate void EffectAction<TEvent>(TEvent e, IPipeline pipeline) where TEvent : IEvent;

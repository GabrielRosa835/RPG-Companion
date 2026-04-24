namespace RpgCompanion.Core;

using Engine;

public interface IAsyncEffect<TEvent> where TEvent : IEvent
{
    public Task ApplyAsync(TEvent e);
}

public delegate Task AsyncEffectAction<TEvent>(TEvent e, IPipeline pipeline) where TEvent : IEvent;

namespace RpgCompanion.Core;

public interface IAsyncEffectCondition<TEvent> where TEvent : IEvent
{
    Task<bool> ShouldApplyAsync(TEvent e);
}

public delegate Task<bool> AsyncEffectCondition<TEvent>(TEvent e) where TEvent : IEvent;

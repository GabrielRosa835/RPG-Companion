namespace RpgCompanion.Host.Delegates;

using Core;

public class AsyncEffectConditionHandler<TEvent>(AsyncEffectCondition<TEvent> action) : IAsyncEffectCondition<TEvent> where TEvent : IEvent
{
    public Task<bool> ShouldApplyAsync(TEvent e) => action(e);
}

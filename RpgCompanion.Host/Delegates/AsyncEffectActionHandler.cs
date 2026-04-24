namespace RpgCompanion.Host.Delegates;

using Core;

public class AsyncEffectActionHandler<TEvent>(AsyncEffectAction<TEvent> action) : IAsyncEffect<TEvent> where TEvent : IEvent
{
    public Task ApplyAsync(TEvent e) => action(e, null!);
}

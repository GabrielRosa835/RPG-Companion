namespace RpgCompanion.Core;

using Engine.Contexts;

public interface IAsyncRule<TEvent> where TEvent : IEvent
{
    Task<TEvent> ApplyAsync(IContext context);
}

public delegate Task<TEvent> AsyncRuleAction<TEvent>(IContext context) where TEvent : IEvent;

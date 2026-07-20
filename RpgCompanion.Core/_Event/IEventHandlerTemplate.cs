namespace RpgCompanion.Core;

public interface IEventHandlerTemplate
{
    static abstract EventResult Handle(IEventContext ctx);
}

public interface IEventHandlerAsyncTemplate
{
    static abstract Task<EventResult> Handle(IEventContext ctx);
}

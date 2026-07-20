namespace RpgCompanion.Core;

public delegate EventResult EventHandler(IEventContext ctx);

public delegate Task<EventResult> EventHandlerAsync(IEventContext ctx);

namespace RpgCompanion.Core;

public delegate EventResult EventHandler(EventContext ctx);

public delegate Task<EventResult> EventHandlerAsync(EventContext ctx);

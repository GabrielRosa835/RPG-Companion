namespace RpgCompanion.Core;

public delegate void InitializationHandler (IInitializationContext context);
public delegate Task InitializationHandlerAsync (IInitializationContextAsync context);

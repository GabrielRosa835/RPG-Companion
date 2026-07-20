namespace RpgCompanion.Core;

public delegate void IntentHandler<in TIntent>(TIntent intent, IIntentContext context) where TIntent : IIntent;

public delegate TResult IntentHandler<in TIntent, TResult>(TIntent intent, IIntentContext context) where TIntent : IIntent<TResult>;

public delegate Task IntentHandlerAsync<in TIntent>(TIntent intent, IIntentContextAsync context) where TIntent : IIntent;

public delegate Task<TResult> IntentHandlerAsync<in TIntent, TResult>(TIntent intent, IIntentContextAsync context) where TIntent : IIntent<TResult>;

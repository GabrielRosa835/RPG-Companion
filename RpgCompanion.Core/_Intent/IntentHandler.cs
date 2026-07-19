namespace RpgCompanion.Core;

public delegate void IntentHandler<in TIntent>(TIntent intent, IntentContext context) where TIntent : IIntent;

public delegate TResult IntentHandler<in TIntent, TResult>(TIntent intent, IntentContext context) where TIntent : IIntent<TResult>;

public delegate Task IntentHandlerAsync<in TIntent>(TIntent intent, IntentContext context, CancellationToken cancellationToken) where TIntent : IIntent;

public delegate Task<TResult> IntentHandlerAsync<in TIntent, TResult>(TIntent intent, IntentContext context, CancellationToken cancellationToken) where TIntent : IIntent<TResult>;

namespace RpgCompanion.Core;

public interface IIntentHandler<in TIntent>  where TIntent : IIntent
{
    Task Handle(TIntent intent, IntentContext context);
}

public interface IIntentHandler<in TIntent, TResult>  where TIntent : IIntent<TResult>
{
    Task<TResult> Handle(TIntent intent, IntentContext context);
}

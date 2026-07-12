namespace RpgCompanion.Core;

public interface IIntentHandler<in TIntent> where TIntent : IIntent
{
    Task Handle(TIntent intent);
}

public interface IIntentHandler<in TIntent, TResult> where TIntent : IIntent<TResult>
{
    Task<TResult> Handle(TIntent intent);
}

public abstract class IntentContext;

public delegate Task<TResult> IntentHandler<in TIntent, TResult>(TIntent intent, IntentContext context);
public delegate Task IntentHandler<in TIntent>(TIntent intent, IntentContext context);

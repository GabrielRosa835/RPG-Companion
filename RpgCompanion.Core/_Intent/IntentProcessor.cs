namespace RpgCompanion.Core;

public interface IIntentProcessor<in TIntent>  where TIntent : IIntent
{
    void Process(TIntent intent, IIntentContext context);
}

public interface IIntentProcessor<in TIntent, TResult>  where TIntent : IIntent<TResult>
{
    TResult Process(TIntent intent, IIntentContext context);
}

public interface IAsyncIntentProcessor<in TIntent>  where TIntent : IIntent
{
    Task Process(TIntent intent, IIntentContext context, CancellationToken cancellationToken);
}

public interface IAsyncIntentProcessor<in TIntent, TResult>  where TIntent : IIntent<TResult>
{
    Task<TResult> Process(TIntent intent, IIntentContext context, CancellationToken cancellationToken);
}

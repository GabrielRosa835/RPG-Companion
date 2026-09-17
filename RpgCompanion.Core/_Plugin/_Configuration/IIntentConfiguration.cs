namespace RpgCompanion.Core;

public interface IIntentConfiguration<TIntent> where TIntent : IIntent
{
    void WithKey(string key);
    void WithName(string name);
    void WithProcessor<TProcessor>() where TProcessor : class, IIntentHandler<TIntent>;
    void WithAsyncProcessor<TProcessor>() where TProcessor : class, IIntentHandler<TIntent>;
}

public interface IIntentConfiguration<TIntent, TResult> where TIntent : IIntent<TResult>
{
    void WithKey(string key);
    void WithName(string name);
    void WithProcessor<TProcessor>() where TProcessor : class, IIntentHandler<TIntent, TResult>;
    void WithAsyncProcessor<TProcessor>() where TProcessor : class, IIntentHandler<TIntent, TResult>;
}

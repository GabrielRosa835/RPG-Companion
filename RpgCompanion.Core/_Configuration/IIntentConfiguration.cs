namespace RpgCompanion.Core;

public interface IIntentConfiguration<TIntent> where TIntent : IIntent
{
    void WithKey(string key);
    void WithName(string name);
    void WithProcessor<TProcessor>() where TProcessor : class, IIntentProcessor<TIntent>;
    void WithAsyncProcessor<TProcessor>() where TProcessor : class, IAsyncIntentProcessor<TIntent>;
}

public interface IIntentConfiguration<TIntent, TResult> where TIntent : IIntent<TResult>
{
    void WithKey(string key);
    void WithName(string name);
    void WithProcessor<TProcessor>() where TProcessor : class, IIntentProcessor<TIntent, TResult>;
    void WithAsyncProcessor<TProcessor>() where TProcessor : class, IAsyncIntentProcessor<TIntent, TResult>;
}

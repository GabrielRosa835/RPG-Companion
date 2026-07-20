namespace RpgCompanion.Core;

public interface IIntentHandlerTemplate<TIntent> where TIntent : IIntent
{
    static abstract void Handle(TIntent intent, IIntentContext context);
}

public interface IIntentHandlerTemplate<TIntent, TResult> where TIntent : IIntent<TResult>
{
    static abstract TResult Handle(TIntent intent, IIntentContext context);
}

public interface IIntentHandlerTemplateAsync<TIntent> where TIntent : IIntent
{
    static abstract Task Handle(TIntent intent, IIntentContextAsync context);
}

public interface IIntentHandlerTemplateAsync<TIntent, TResult> where TIntent : IIntent<TResult>
{
    static abstract Task<TResult> Handle(TIntent intent, IIntentContextAsync context);
}

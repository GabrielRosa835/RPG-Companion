namespace RpgCompanion.Core;

public interface IInitializationHandlerTemplate
{
    static abstract void Handle (IInitializationContext context);
}

public interface IInitializationHandlerAsyncTemplate
{
    static abstract Task Handle (IInitializationContextAsync context);
}

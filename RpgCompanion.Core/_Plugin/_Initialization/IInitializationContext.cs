namespace RpgCompanion.Core;

public interface IInitializationContext
{
    IRegistry Registry { get; }
}

public interface IInitializationContextAsync : IInitializationContext
{
    CancellationToken CancellationToken { get; }
}

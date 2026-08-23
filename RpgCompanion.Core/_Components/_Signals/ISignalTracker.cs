namespace RpgCompanion.Core;

public interface ISignalTracker
{
    Task WaitAllAsync(CancellationToken cancellationToken);
}

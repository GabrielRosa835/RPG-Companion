namespace RpgCompanion.Core;

public interface ISignalSender
{
    public ISignalTracker Send(ISignal signal);
}

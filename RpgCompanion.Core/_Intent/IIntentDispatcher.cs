namespace RpgCompanion.Core;

public interface IIntentDispatcher
{
    Task Dispatch(IIntent intent, CancellationToken cancellationToken = default);
    Task<TResult> Dispatch<TResult>(IIntent<TResult> intent, CancellationToken cancellationToken = default);
}

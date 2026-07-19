namespace RpgCompanion.Core;

public interface IIntentDispatcher
{
    IntentTask Dispatch(IIntent intent, CancellationToken cancellationToken = default);
    IntentTask<TResult> Dispatch<TResult>(IIntent<TResult> intent, CancellationToken cancellationToken = default);
}

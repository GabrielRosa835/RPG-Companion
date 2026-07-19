namespace RpgCompanion.Core;

using System.Runtime.CompilerServices;

public record IntentTask(Task ExecutionTask)
{
    public TaskAwaiter GetAwaiter() => ExecutionTask.GetAwaiter();

    /*
     * More metadata I may add later...
     */
}

public record IntentTask<TResult>(Task<TResult> ExecutionTask)
{
    public TaskAwaiter<TResult> GetAwaiter() => ExecutionTask.GetAwaiter();

    /*
     * More metadata I may add later...
     */
}

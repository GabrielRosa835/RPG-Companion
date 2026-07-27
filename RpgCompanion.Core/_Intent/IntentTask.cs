namespace RpgCompanion.Core;

using System.Runtime.CompilerServices;

public record IntentTask(Task Task)
{
    public TaskAwaiter GetAwaiter() => Task.GetAwaiter();
    public static implicit operator Task(IntentTask intentTask) => intentTask.Task;

    /*
     * More metadata I may add later...
     */
}

public record IntentTask<TResult>(Task<TResult> Task)
{
    public TaskAwaiter<TResult> GetAwaiter() => Task.GetAwaiter();
    public static implicit operator Task<TResult>(IntentTask<TResult> intentTask) => intentTask.Task;

    /*
     * More metadata I may add later...
     */
}

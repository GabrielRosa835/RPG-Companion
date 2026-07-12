namespace RpgCompanion.Core;

using System.Runtime.CompilerServices;

public record EventTask(Task<EventResult> ExecutionTask)
{
    public TaskAwaiter<EventResult> GetAwaiter() => ExecutionTask.GetAwaiter();

    /*
     * More metadata I may add later...
     */
}

namespace RpgCompanion.Core;

using System.Runtime.CompilerServices;

public record EventTask(Task ExecutionTask)
{
    public TaskAwaiter GetAwaiter() => ExecutionTask.GetAwaiter();

    public object Result { get; set; } = default!;

    /*
     * More metadata I may add later...
     */
}

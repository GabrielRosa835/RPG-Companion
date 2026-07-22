namespace RpgCompanion.Core;

using System.Runtime.CompilerServices;

public record EventTask(Task<EventResult> Task)
{
    public TaskAwaiter<EventResult> GetAwaiter() => Task.GetAwaiter();
    public static implicit operator Task<EventResult>(EventTask eventTask) => eventTask.Task;

    public object Result { get; set; } = default!;

    /*
     * More metadata I may add later...
     */
}

namespace RpgCompanion.Canva;

using Core;

public class Event1 : Event
{
    private int _passes = 0;

    public override ValueTask Setup(IEventContext context, CancellationToken CancellationToken)
    {
        Log.Debug("{0} (Thread {1}): {2}", this.GetType().Name, Thread.CurrentThread.ManagedThreadId, nameof(Setup));
        return Completed;
    }
    public override ValueTask Execute(IEventContext context, CancellationToken CancellationToken)
    {
        Log.Debug("{0} (Thread {1}): {2}", this.GetType().Name, Thread.CurrentThread.ManagedThreadId, nameof(Execute));
        if (++_passes >= 3) context.Continue(new Event2());
        return Completed;
    }
    public override ValueTask Teardown(IEventContext context, CancellationToken CancellationToken)
    {
        Log.Debug("{0} (Thread {1}): {2}", this.GetType().Name, Thread.CurrentThread.ManagedThreadId, nameof(Teardown));
        return Completed;
    }
}

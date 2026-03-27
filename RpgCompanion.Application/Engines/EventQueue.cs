namespace RpgCompanion.Application;

using Services;

internal class EventQueue
{
    private readonly PriorityQueue<EventItem, int> _queue = new();

    public EventItem Dequeue() => _queue.Dequeue();

    public void Enqueue(EventItem item) => _queue.Enqueue(item, item.Priority);

    public void EnqueueRange(IEnumerable<EventItem> items) =>
        _queue.EnqueueRange(items.Select(i => (i, i.Priority)));

    public int Count  => _queue.Count;
}

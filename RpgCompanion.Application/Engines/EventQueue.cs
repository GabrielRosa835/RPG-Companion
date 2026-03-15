namespace RpgCompanion.Application;

using RpgCompanion.Application.Services;

internal class EventQueue
{
    private readonly PriorityQueue<EventDescriptor, int> _queue = new();

    public EventDescriptor Dequeue()
    {
        return _queue.Dequeue();
    }
    public void Enqueue(EventDescriptor descriptor)
    {
        _queue.Enqueue(descriptor, descriptor.EffectivePriority);
    }
    public void EnqueueRange(IEnumerable<EventDescriptor> descriptors)
    {
        _queue.EnqueueRange(descriptors.Select(descriptor => (descriptor, descriptor.EffectivePriority)));
    }
    public bool Any()
    {
        return _queue.Count > 0;
    }
}

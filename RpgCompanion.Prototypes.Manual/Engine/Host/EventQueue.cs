namespace RpgCompanion.Application;

using Services;

internal class EventQueue : Queue<EventItem>
{
    internal void EnqueueRange(IEnumerable<EventItem> events)
    {
        foreach (var e in events)
        {
            Enqueue(e);
        }
    }
}

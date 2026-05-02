namespace RpgCompanion.Host;

using Core;

internal class Pipeline<TEvent>(Queue<Func<IEvent, IEvent>> _queue) : IPipeline<TEvent> where TEvent : IEvent
{
    public IPipeline<TNext> Then<TNext>(Func<TEvent, TNext> continuation) where TNext : IEvent
    {
        _queue.Enqueue(e => continuation((TEvent) e));
        return new Pipeline<TNext>(_queue);
    }
}

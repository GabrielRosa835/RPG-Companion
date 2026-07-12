namespace RpgCompanion.Host;

using Events;

public static class ComponentQueries
{
    extension(IQueryable<EventDescriptor> query)
    {
        public EventDescriptor Find<TEvent>() where TEvent : IEvent
        {
            return query.Find(typeof(TEvent));
        }
        public EventDescriptor Find(Type eventType)
        {
            return query.FirstOrDefault(d => d.Type == eventType)
                 ?? throw new InvalidOperationException(
                     $"Could not find a descriptor for event of type {eventType.Name}");
        }
    }
}

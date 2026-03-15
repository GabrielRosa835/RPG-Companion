namespace RpgCompanion.Application;

using RpgCompanion.Application.Services;
using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;

internal class EventBuilder<TEvent>(EventDescriptor eventDescriptor, ComponentCollection components) : IEventBuilder<TEvent> where TEvent : IEvent
{
    public IEventBuilder<TEvent> WithName(string name)
    {
        eventDescriptor.DisplayName = name;
        return this;
    }
    public IEventBuilder<TEvent> WithPriority(int priority)
    {
        eventDescriptor.Priority = priority;
        return this;
    }
    public IEventComponentBuilder<TEvent> WithComponents()
    {
        return new EventComponentBuilder<TEvent>(components);
    }
}

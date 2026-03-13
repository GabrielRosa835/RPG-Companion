using RpgCompanion.Application;
using RpgCompanion.Core.Events;

namespace RpgCompanion.Core.Engine;

public interface IEventBuilder<TEvent> where TEvent : IEvent
{
    public IEventBuilder<TEvent> WithName(string name);
    public IEventBuilder<TEvent> WithPriority(int priority);
    public IEventComponentBuilder<TEvent> WithComponents();
}
namespace RpgCompanion.Core;

public interface IEventConfiguration<TEvent> where TEvent : Event
{
    void WithKey(EventKey key);
    void WithName(string name);
}

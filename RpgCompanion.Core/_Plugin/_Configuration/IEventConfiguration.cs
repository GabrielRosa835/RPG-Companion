namespace RpgCompanion.Core;

public interface IEventConfiguration<TEvent> where TEvent : Event
{
    void WithKey(string key);
    void WithName(string name);
}

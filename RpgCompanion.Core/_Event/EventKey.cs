namespace RpgCompanion.Core;

public readonly record struct EventKey(string Content)
{
    public static implicit operator EventKey(string content) => new(content);
}

public readonly record struct EventKey<TEvent>(string Content) where TEvent : IEvent
{
    public static implicit operator EventKey<TEvent>(string content) => new(content);
    public static implicit operator EventKey(EventKey<TEvent> key) => new(key.Content);
}

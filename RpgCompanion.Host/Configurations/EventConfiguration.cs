namespace RpgCompanion.Host;

internal class EventConfiguration<TEvent>(
    PluginKey _pluginKey,
    EventArchives _eventArchives,
    IServiceCollection _services)
    : IEventConfiguration<TEvent> where TEvent : Event
{
    private EventKey _key = new(Guid.CreateVersion7().ToString());
    private string? _name;

    internal void Commit()
    {
        var descriptor = new EventDescriptor
        {
            PluginKey = _pluginKey,
            Key = _key,
            Name = _name,
            Type = typeof(TEvent),
        };
        _services.AddKeyedSingleton(_key, descriptor);
        _eventArchives.Add(descriptor);
    }

    public void WithKey(string key)
    {
        _key = new EventKey(key);
    }

    public void WithName(string name)
    {
        _name = name;
    }
}

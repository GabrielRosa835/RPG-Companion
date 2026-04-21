namespace RpgCompanion.Host.Descriptors;

using Plugins;

internal class PluginDescriptor
{
    internal PluginKey Key { get; init; }
    internal string? Name { get; init; }
    internal string? Version { get; init; }
    internal IReadOnlySet<EventKey> Events { get; init; }
    internal PluginMetadata Metadata { get; init; }
}

public readonly record struct PluginKey(string Value)
{
    public PluginKey() : this(Guid.NewGuid().ToString())
    {
    }
}

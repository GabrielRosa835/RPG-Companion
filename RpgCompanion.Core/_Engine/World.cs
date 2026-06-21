namespace RpgCompanion.Core;

using Toolbox;

public class World
{
    public required PluginKey Plugin { get; init; }
    public IStorage Variables { get; } = new ConcurrentDynamicStorage();
}

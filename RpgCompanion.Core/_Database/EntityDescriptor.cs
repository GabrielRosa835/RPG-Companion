namespace RpgCompanion.Core;

public class EntityDescriptor
{
    public PluginKey PluginKey { get; init; }
    public EntityKey Key { get; init; }
    public Type Type { get; init; } = default!;
    public string? Collection { get; init; }
    public string? Name { get; init; }
}

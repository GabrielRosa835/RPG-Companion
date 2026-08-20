namespace RpgCompanion.Core;

public class IntentDescriptor
{
    public PluginKey PluginKey { get; init; }
    public IntentKey Key { get; init; } = default!;
    public string? Name { get; init; }
    public Type Type { get; init; } = default!;
    public Type ProcessorType { get; init; } = default!;
}

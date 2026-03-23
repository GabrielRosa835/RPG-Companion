namespace RpgCompanion.Application;

using Core.Meta;

internal class PluginMetadata
{
    internal Type? InitializerType { get; set; }
    internal InitializerAction? Initialization { get; set; }
    internal string? PluginName { get; set; }
    internal string? PluginVersion { get; set; }
}

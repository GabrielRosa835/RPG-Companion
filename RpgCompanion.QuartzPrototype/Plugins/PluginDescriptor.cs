namespace RpgCompanion.QuartzPrototype.Plugins;

using System.Reflection;
using Descriptors;

internal class PluginDescriptor
{
    internal string FilePath { get; init; }
    internal string Resource { get; init; }

    internal bool Activated { get; set; }
    internal PluginIdentifier Identifier { get; set; } = default!;
    internal Assembly Assembly { get; set; } = default!;
    internal IServiceProvider Services { get; set; } = default!;
    internal List<EventKey> Events { get; } = [];

    public override string ToString() => this.Activated ? this.Identifier.ToString() : this.Resource;

    internal PluginDescriptor(string path)
    {
        this.Resource = Path.GetFileNameWithoutExtension(path);
        this.FilePath = Path.GetFullPath(path);
    }
}

internal record PluginIdentifier
{
    public required string Name { get; init; }
    public required string Version { get; init; }
}

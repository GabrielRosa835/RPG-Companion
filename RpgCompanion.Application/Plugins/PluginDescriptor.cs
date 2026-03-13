using RpgCompanion.Core.Meta;

using System.Reflection;

namespace RpgCompanion.Application;

internal class PluginDescriptor : IEquatable<PluginDescriptor>
{
    internal string Path { get; init; } = default!;
    internal string Resource { get; init; } = default!;
    
    internal bool Activated { get; set; }
    internal PluginIdentifier Identifier { get; set; } = default!;
    internal Assembly Assembly { get; set; } = default!;
    internal ComponentProvider Registry { get; set; } = default!;
    
    public bool Equals(PluginDescriptor? other) => Resource == other?.Resource;
    public override bool Equals(object? obj) => obj is  PluginDescriptor other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Path);
    public override string ToString() => Activated ? Identifier.ToString() : Resource;
}

internal record PluginIdentifier(string? Name, string? Version);
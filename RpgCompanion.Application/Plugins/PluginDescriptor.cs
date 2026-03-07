using RpgCompanion.Core.Meta;

using System.Reflection;

namespace RpgCompanion.Application;

internal class PluginDescriptor : IEquatable<PluginDescriptor>
{
    internal string Path { get; init; } = default!;
    internal string Name { get; init; } = default!;
    
    internal bool Activated { get; set; }
    internal IPlugin System { get; set; } = default!;
    internal Assembly Assembly { get; set; } = default!;
    internal ComponentProvider Registry { get; set; } = default!;
    internal IServiceProvider Provider { get; set; } = default!;
    
    public bool Equals(PluginDescriptor? other)
    {
        return Path == other?.Path && Name == other?.Name;
    }
}
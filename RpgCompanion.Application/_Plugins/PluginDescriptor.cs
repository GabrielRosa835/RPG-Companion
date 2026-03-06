using System.Reflection;

using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Meta;

namespace RpgCompanion.Application;

internal class PluginDescriptor
{
    internal string Path { get; init; } = default!;
    internal string Name { get; init; } = default!;
    
    internal bool Activated { get; set; }
    internal ISystem System { get; set; } = default!;
    internal Assembly Assembly { get; set; } = default!;
    internal IRegistry Registry { get; set; } = default!;
}
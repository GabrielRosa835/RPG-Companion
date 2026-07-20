namespace RpgCompanion.Core;

/// <summary>
/// Plugin being currently executed
/// </summary>
public interface IPluginContext
{
    public PluginKey Key { get; }
}

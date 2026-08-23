namespace RpgCompanion.Core;

/// <summary>
/// Plugin being currently executed
/// </summary>
public interface IPluginContext
{
    public string Identifier { get; }
}

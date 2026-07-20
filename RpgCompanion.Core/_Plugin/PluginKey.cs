namespace RpgCompanion.Core;

public readonly record struct PluginKey(string Content)
{
    public static implicit operator PluginKey(string content) => new(content);
}

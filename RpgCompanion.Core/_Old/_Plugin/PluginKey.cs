namespace RpgCompanion.Events;

public readonly record struct PluginKey(string Content)
{
    public static implicit operator PluginKey(string content) => new(content);
}

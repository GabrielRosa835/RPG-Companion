namespace RpgCompanion.Host;

public readonly record struct PluginKey(string Value)
{
    public PluginKey() : this(Guid.NewGuid().ToString())
    {
    }
}

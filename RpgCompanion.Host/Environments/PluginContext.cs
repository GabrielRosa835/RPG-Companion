namespace RpgCompanion.Host;

public class PluginContext : IPluginContext
{
    public string Identifier { get; internal set; } = default!;
}

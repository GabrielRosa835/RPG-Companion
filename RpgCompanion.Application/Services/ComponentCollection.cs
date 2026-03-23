namespace RpgCompanion.Application.Services;

internal class ComponentCollection
{
    public List<BundlerDescriptor> Bundlers { get; } = [];
    public List<CustomDescriptor> Customs { get; } = [];
    public List<EffectDescriptor> Effects { get; } = [];
    public List<EventDescriptor> Events { get; } = [];
    public List<RuleDescriptor> Rules { get; } = [];

    public ComponentCollection() {}
    public ComponentCollection(ComponentCollection other)
    {
        this.Bundlers.AddRange(other.Bundlers);
        this.Customs.AddRange(other.Customs);
        this.Effects.AddRange(other.Effects);
        this.Events.AddRange(other.Events);
        this.Rules.AddRange(other.Rules);
    }
}

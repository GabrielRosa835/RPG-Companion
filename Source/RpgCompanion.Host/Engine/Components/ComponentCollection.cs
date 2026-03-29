namespace RpgCompanion.Application.Services;

internal class ComponentCollection
{
    public List<CustomDescriptor> Customs { get; } = [];
    public List<EffectDescriptor> Effects { get; } = [];
    public List<EventDescriptor> Events { get; } = [];
    public List<RuleDescriptor> Rules { get; } = [];
}

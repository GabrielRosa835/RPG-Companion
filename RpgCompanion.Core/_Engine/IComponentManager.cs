namespace RpgCompanion.Core;

public interface IComponentManager
{
    PluginDescriptor FindPlugin(PluginKey key);
    EventDescriptor FindEvent(EventKey key);
    ActorDescriptor FindActor(ActorKey key);
    RuleDescriptor FindRule(RuleKey key);
}

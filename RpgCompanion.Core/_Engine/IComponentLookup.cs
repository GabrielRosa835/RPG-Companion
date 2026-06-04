namespace RpgCompanion.Core;

public interface IComponentLookup
{
    PluginDescriptor? Find(PluginKey key);
    EventDescriptor? Find(EventKey key);
    ActorDescriptor? Find(ActorKey key);
    RuleDescriptor? Find(RuleKey key);

    PluginDescriptor Get(PluginKey key);
    EventDescriptor Get(EventKey key);
    ActorDescriptor Get(ActorKey key);
    RuleDescriptor Get(RuleKey key);
}

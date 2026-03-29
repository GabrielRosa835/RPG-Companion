using Utils.UnionTypes;

namespace RpgCompanion.Shared;

public interface IPluginManager
{
    ComponentProvider this[Type eventType] { get; }
    Task<Attempt> Load(IPluginDescriptor pluginDescriptor);
}

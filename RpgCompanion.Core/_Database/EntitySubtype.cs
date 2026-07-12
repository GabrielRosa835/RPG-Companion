namespace RpgCompanion.Core;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
public class EntitySubtypeAttribute : Attribute
{
    public Type KnownType { get; }
    public EntitySubtypeAttribute(Type knownType) => KnownType = knownType;
}

public interface IPluginConfiguration2
{
    void RegisterPolymorphicHierarchy<TBase>(params Type[] derivedTypes);
}

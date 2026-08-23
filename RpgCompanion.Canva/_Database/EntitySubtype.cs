namespace RpgCompanion.Core;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
public class HasSubtypeAttribute : Attribute
{
    public Type KnownType { get; }
    public HasSubtypeAttribute(Type knownType) => KnownType = knownType;
}

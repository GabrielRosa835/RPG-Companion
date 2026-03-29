namespace RpgCompanion.Core.Meta;

public interface IInitializationBuilder
{
    public IInitializationBuilder WithComponent<TInitializer>() where TInitializer : class, IInitializer;
    public IInitializationBuilder WithAction(InitializerAction action);
}

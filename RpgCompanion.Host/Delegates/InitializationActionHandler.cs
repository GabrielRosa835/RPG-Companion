namespace RpgCompanion.Host.Delegates;

using Core.Engine;
using Core.Meta;

internal class InitializationActionHandler(InitializerAction action) : IInitializer
{
    public void Initialize(IRegistry registry) => action(registry);
}

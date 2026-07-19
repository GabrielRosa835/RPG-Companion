namespace RpgCompanion.Host.Intents;

public class IntentContextImpl(
    IServiceScope _scope,
    IRegistry _registry)
    : IntentContext, IDisposable
{
    public override IRegistry Registry => _registry;

    public void Dispose()
    {
        _scope.Dispose();
    }
}

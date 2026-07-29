namespace RpgCompanion.Host.HostExclusive;

internal class HostContext(HostRegistry _registry) : IHostContext
{
    public IHostRegistry Registry => _registry;
}

namespace RpgCompanion.Core;

public interface ISignalTargetPolicy
{
    public IEnumerable<ClientId> GetClientIds(ISignalContext context);
}

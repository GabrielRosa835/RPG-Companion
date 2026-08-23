namespace RpgCompanion.Core;

internal class BroadcastPolicy : IQuestionTargetPolicy, ISignalTargetPolicy
{
    public IEnumerable<ClientId> GetClientIds(IQuestionContext context)
    {
        return context.Session.AllPlayers.Select(p => p.ClientId);
    }

    public IEnumerable<ClientId> GetClientIds(ISignalContext context)
    {
        return context.Session.AllPlayers.Select(p => p.ClientId);
    }
}

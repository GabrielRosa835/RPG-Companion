namespace RpgCompanion.Core;

internal class SingleTargetPolicy(IPlayer target) : IQuestionTargetPolicy, ISignalTargetPolicy
{
    public IEnumerable<ClientId> GetClientIds(IQuestionContext context)
    {
        if (context.Session.AllPlayers.All(p => p.ClientId != target.ClientId))
        {
            throw new InvalidOperationException();
        }
        yield return target.ClientId;
    }

    public IEnumerable<ClientId> GetClientIds(ISignalContext context)
    {
        if (context.Session.AllPlayers.All(p => p.ClientId != target.ClientId))
        {
            throw new InvalidOperationException();
        }
        yield return target.ClientId;
    }
}

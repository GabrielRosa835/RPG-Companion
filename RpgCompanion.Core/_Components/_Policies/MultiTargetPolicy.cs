namespace RpgCompanion.Core;

internal class MultiTargetPolicy(IEnumerable<IPlayer> targets) : IQuestionTargetPolicy, ISignalTargetPolicy
{
    public IEnumerable<ClientId> GetClientIds(IQuestionContext context)
    {
        var _targets = targets.ToList();
        if (targets.Any(t => context.Session.AllPlayers.All(p => p.ClientId != t.ClientId)))
        {
            throw new InvalidOperationException();
        }
        return _targets.Select(p => p.ClientId);
    }

    public IEnumerable<ClientId> GetClientIds(ISignalContext context)
    {
        var _targets = targets.ToList();
        if (targets.Any(t => context.Session.AllPlayers.All(p => p.ClientId != t.ClientId)))
        {
            throw new InvalidOperationException();
        }
        return _targets.Select(p => p.ClientId);
    }
}

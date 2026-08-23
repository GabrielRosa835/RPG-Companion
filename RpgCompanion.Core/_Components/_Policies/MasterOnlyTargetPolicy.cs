namespace RpgCompanion.Core;

internal class MasterOnlyTargetPolicy : IQuestionTargetPolicy, ISignalTargetPolicy
{
    public IEnumerable<ClientId> GetClientIds(IQuestionContext context)
    {
        yield return context.Session.SessionMaster.ClientId;
    }

    public IEnumerable<ClientId> GetClientIds(ISignalContext context)
    {
        yield return context.Session.SessionMaster.ClientId;
    }
}

namespace RpgCompanion.Core;

public interface IQuestionTargetPolicy
{
    public IEnumerable<ClientId> GetClientIds(IQuestionContext context);
}

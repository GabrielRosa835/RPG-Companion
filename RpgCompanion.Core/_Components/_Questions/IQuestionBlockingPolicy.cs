namespace RpgCompanion.Core;

public interface IQuestionBlockingPolicy
{
    public IEnumerable<ClientId> GetClientsIds(IQuestionContext context);
}

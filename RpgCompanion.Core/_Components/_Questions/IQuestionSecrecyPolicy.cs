namespace RpgCompanion.Core;

public interface IQuestionSecrecyPolicy
{
    public IEnumerable<IGrouping<ClientId, ClientId>> GetClientsIds(IQuestionContext context);
}

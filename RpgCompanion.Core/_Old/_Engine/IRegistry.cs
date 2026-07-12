namespace RpgCompanion.Core;

public interface IRegistry
{
    public TService? Find<TService>() where TService : class;
    public TService Get<TService>() where TService : class;
}

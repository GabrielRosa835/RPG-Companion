namespace RpgCompanion.Core;

public interface IRegistry
{
    TService? Find<TService>() where TService : class;
    TService Get<TService>() where TService : class;
}

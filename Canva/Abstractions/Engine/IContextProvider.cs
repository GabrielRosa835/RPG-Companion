namespace RpgCompanion.Canva;

public interface IContextProvider
{
   IContext Bundle(IEvent @event);
}

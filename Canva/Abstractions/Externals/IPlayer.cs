namespace RpgCompanion.Canva;

public interface IPlayer
{
   IOption ChooseFrom(OptionList options, IContext context);
   T ChooseFrom <T>(OptionList<T> options, IContext context);
}

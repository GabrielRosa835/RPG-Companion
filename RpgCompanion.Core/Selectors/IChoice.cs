using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Options;

namespace RpgCompanion.Core.Selectors;

public interface IChoice
{
   OptionList Select (IContext context);
   public static IChoice Of (Func<IContext, OptionList> f) => new ChoiceDelegate(f);
}

internal class ChoiceDelegate (Func<IContext, OptionList> @delegate) : IChoice
{
   public OptionList Select (IContext context) => @delegate(context);
}

public interface IChoice<T>
{
   OptionList<T> Select (IContext context);
   public static IChoice<T> Of (Func<IContext, OptionList<T>> f) => new ChoiceDelegate<T>(f);
}

internal class ChoiceDelegate <T>(Func<IContext, OptionList<T>> @delegate) : IChoice<T>
{
   public OptionList<T> Select (IContext context) => @delegate(context);
}
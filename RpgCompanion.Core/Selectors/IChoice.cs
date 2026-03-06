using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Options;

namespace RpgCompanion.Core.Selectors;

public interface IChoice
{
   OptionList Select (Context context);
   public static IChoice Of (Func<Context, OptionList> f) => new ChoiceDelegate(f);
}

internal class ChoiceDelegate (Func<Context, OptionList> @delegate) : IChoice
{
   public OptionList Select (Context context) => @delegate(context);
}

public interface IChoice<T>
{
   OptionList<T> Select (Context context);
   public static IChoice<T> Of (Func<Context, OptionList<T>> f) => new ChoiceDelegate<T>(f);
}

internal class ChoiceDelegate <T>(Func<Context, OptionList<T>> @delegate) : IChoice<T>
{
   public OptionList<T> Select (Context context) => @delegate(context);
}
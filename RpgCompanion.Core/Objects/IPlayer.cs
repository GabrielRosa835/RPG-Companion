using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Selectors;

namespace RpgCompanion.Core.Objects;

public interface IPlayer
{
   object ChooseFrom(IChoice options, Context context);
   T ChooseFrom <T>(IChoice<T> options, Context context);
}

using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Objects;

namespace RpgCompanion.Core.Selectors;

public interface ITarget
{
   IObject Select(Context context);

   public static ITarget Of (Func<Context, IObject> @delegate) => new TargetDelegate(@delegate);
}

internal class TargetDelegate (Func<Context, IObject> @delegate) : ITarget
{
   public IObject Select (Context context) => @delegate(context);
}
using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Objects;

namespace RpgCompanion.Core.Selectors;

public interface ITarget
{
   IObject Select(IContext context);

   public static ITarget Of (Func<IContext, IObject> @delegate) => new TargetDelegate(@delegate);
}

internal class TargetDelegate (Func<IContext, IObject> @delegate) : ITarget
{
   public IObject Select (IContext context) => @delegate(context);
}
using RpgCompanion.Core.Objects;

namespace RpgCompanion.Core.Selectors;

public interface ITargetGroup
{
   ICollection<IObject> Select(IObject obj);
}

using RpgCompanion.Core.Events;
using RpgCompanion.Core.Objects;

namespace RpgCompanion.Core.Engine;

internal interface IContextSnapshot
{
   IEvent Trigger { get; }
   IScene Scene { get; }
   IReadOnlyList<IObject> Objects { get; }
   IReadOnlyList<IPlayer> Players { get; }
}

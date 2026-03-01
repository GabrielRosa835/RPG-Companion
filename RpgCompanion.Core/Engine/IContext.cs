using RpgCompanion.Core.Events;
using RpgCompanion.Core.Objects;

namespace RpgCompanion.Core.Engine;

public interface IContext
{
   IEngine Engine { get; }
   IEvent Trigger { get; }
   IReadOnlyList<IObject> Objects { get; }
   Dictionary<string, dynamic> SharedData { get; }
}
using RpgCompanion.Core.Events;

namespace RpgCompanion.Core.Engine;

public interface IEngine
{
   IContextProvider ContextProvider { get; }
   IEventQueue EventQueue { get; }
   IRegistry Registry { get; }
}

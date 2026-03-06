using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Utils;

namespace RpgCompanion.Core.Events;

public interface IEventHandler<in TEvent> where TEvent : IEvent
{
   void Handle (TEvent @event, IContext context);
}
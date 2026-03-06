using RpgCompanion.Core.Events;

namespace RpgCompanion.Core.Engine;

internal class Context : IContext
{
   internal readonly ContextData _data = new();

   public IContextData Data => _data;

   public void Raise (IEvent @event)
   {

   }
}
using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Events.Producers;
using RpgCompanion.Core.Objects;
using RpgCompanion.Core.Utils;

namespace RpgCompanion.DnD;

internal record AttackActionEvent (IEvent<AttackAction> Inner) : IEvent<AttackAction>;

internal class AttackAction (IAction<IEvent<AttackAction>> action) : IAction<AttackActionEvent>
{
   public AttackActionEvent For (IActor actor, IContext context)
   {
      var inner = action.For(actor, context);
      return new AttackActionEvent(inner);
   }

   public void Test()
   {
      var @event = new AttackAction(new MeleeAttack()).For(null!, null!);
      @event.Inner.As<MeleeAttackEvent>();
   }
}

internal class AttackActionEventHandler : IEventHandler<AttackActionEvent>
{
   public void Handle (AttackActionEvent @event, IContext context)
   {
      if (@event.Inner is MeleeAttackEvent meleeAttackEvent)
      {
         
      }
   }
}

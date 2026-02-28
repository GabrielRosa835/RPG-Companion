using RpgCompanion.Canva;
using RpgCompanion.Canva.Utils;

using System.Numerics;

using ActionBase = RpgCompanion.Canva.Action;

namespace RpgCompanion.DnD;


public static class MeleeAttack
{
   private static IPlayer CurrentPlayer(this IContext context)
   {
      return null!;
   }
   private static OptionList<IObject> GetObjectsInRange(this IContext context, IActor actor)
   {
      return new OptionList<IObject>();
   }
   private static ITarget GetTarget(this IContext context, IActor actor, IPlayer player)
   {
      return ITarget.Of(context => player.ChooseFrom(context.GetObjectsInRange(actor), context));
   }

   internal class Event (IActor attacker, ITarget target, DiceRoll.Event attempt, Weapon weapon) : IEvent
   {
      public static Event Create(IActor actor, IContext context)
      {
         return new Event(actor, context.GetTarget(actor, context.CurrentPlayer()), new DiceRoll.Event(new Dice.D20()), new Weapon());
      }
   }

   public class Action : ActionBase
   {
      public override void For (IActor actor, IContext context)
      {
         var @event = Event.Create(actor, context);
         context.Engine.EventQueue.Push(@event);
      }
   }
}
using RpgCompanion.Core.Contexts;
using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;
using RpgCompanion.DnD.Canva;

namespace RpgCompanion.DnD;

public record Heal(int Value, Defender target) : IEvent
{
   public class Effect : IEffect<Heal>
   {
      public void Apply (Heal @event, IPipeline context)
      {
         @event.target.Health += @event.Value;
         if (@event.target.Health > @event.target.MaxHealth)
         {
            @event.target.Health = @event.target.MaxHealth;
         }
      }

      public bool ShouldApply (Heal @event)
      {
         return @event.target.Health < @event.target.MaxHealth;
      }
   }
   public class Packager : IPackager<Heal>
   {
      public void Pack (Heal @event, IEditableContext context)
      {
         throw new NotImplementedException();
      }
   }
}

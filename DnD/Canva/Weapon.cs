using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Events.Producers;
using RpgCompanion.Core.Objects;

namespace RpgCompanion.DnD;

public class Weapon : IObject
{
   public int Damage { get; set; }

   public void Accept<TEvent> (IEffect<TEvent> effect, Context context) where TEvent : IEvent<IEffect<TEvent>>
   {
      throw new NotImplementedException();
   }
}

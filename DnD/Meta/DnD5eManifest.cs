using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Events.Producers;
using RpgCompanion.Core.Meta;
using RpgCompanion.DnD.Meta;

namespace RpgCompanion.DnD;

internal class DnD5eManifest : IManifest<DnD5e>
{
   public Type Initializer => typeof(DnD5eInitializer);

   public void Setup (IRegistryCollection collection)
   {
      collection.Add<IRule<DiceRollEvent>, DiceRoll>();
      collection.Add<IEventHandler<DiceRollEvent>, DiceRollHandler>();
   }

}

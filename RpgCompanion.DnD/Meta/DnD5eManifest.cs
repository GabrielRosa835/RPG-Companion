using RpgCompanion.Application;
using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Meta;
using RpgCompanion.DnD.Canva;

namespace RpgCompanion.DnD;

internal class DnD5eManifest : IManifest
{
   public void Setup (IPluginBuilder builder)
   {
      builder.WithName("D&D 5e")
          .WithInitialization(_ => Console.WriteLine(nameof(DnD5eManifest)))
          .WithVersion("1.0.0");
      builder.AddEvent<DiceRoll>()
          .WithPriority(EventPriorities.Medium)
          .WithName("dice-roll")
          .WithComponents()
              .AddEffect<DiceRoll.Effect>();
      builder.AddEvent<MeleeAttackAction>()
          .WithPriority(EventPriorities.High)
          .WithComponents()
              .AddEffect<MeleeAttackAction.Effect>()
               .AddEffect<AttackAction.Effect>();
      builder.AddEvent<Damage>()
          .WithPriority(1000)
          .WithName("damage")
          .WithComponents()
              .AddEffect<DamageEffect>();
   }
}
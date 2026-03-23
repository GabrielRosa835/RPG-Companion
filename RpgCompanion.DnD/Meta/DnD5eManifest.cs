using RpgCompanion.Application;
using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Meta;
using RpgCompanion.DnD.Canva;

namespace RpgCompanion.DnD;

internal class DnD5eManifest : IManifest
{
    public void Configure(IPluginBuilder builder) => builder
        .WithMetadata(metadata => metadata
            .WithName("D&D 5e")
            .WithInitialization(_ => Console.WriteLine(nameof(DnD5eManifest)))
            .WithVersion("1.0.0"))
        .AddEvent<DiceRoll>(e => e
            .WithName("dice-roll")
            .WithPriority(EventPriorities.Medium)
            .AddEffect(effect => effect
                .WithComponent<DiceRoll.Effect>()))
        .AddEvent<MeleeAttackAction>(e => e
            .WithPriority(EventPriorities.High)
            .AddEffect(effect => effect
                .WithComponent<MeleeAttackAction.Effect>()))
        .AddEvent<Damage>(e => e
            .WithName("damage")
            .WithPriority(1000)
            .AddEffect(effect => effect
                .WithComponent<DamageEffect>()));
}

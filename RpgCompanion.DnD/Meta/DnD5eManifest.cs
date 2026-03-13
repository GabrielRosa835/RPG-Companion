using RpgCompanion.Application;
using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Meta;

namespace RpgCompanion.DnD;

internal class DnD5eManifest : IManifest
{
    public void Setup(IPluginBuilder builder)
    {
        builder.WithName("D&D 5e")
            .WithInitialization(_ => Console.WriteLine(nameof(DnD5eManifest)))
            .WithVersion("1.0.0");
        builder.AddEvent<DiceRoll>()
            .WithPriority(EventPriorities.Medium)
            .WithName("dice-roll")
            .WithComponents()
                .AddEffect<DiceRollEffect>();
        builder.AddEvent<AttackAction>()
            .WithPriority(EventPriorities.High)
            .WithComponents()
                .AddEffect<AttackActionEffect>();
        builder.AddEvent<Damage>()
            .WithPriority(1000)
            .WithName("damage")
            .WithComponents()
                .AddEffect<DamageEffect>();
    }
}
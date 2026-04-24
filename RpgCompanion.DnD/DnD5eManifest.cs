namespace RpgCompanion.DnD;

using Canva;
using Core;

public class DnD5eManifest : IManifest
{
    public void Configure(IPluginConfiguration plugin) => plugin
        .WithName("D&D 5e")
        .WithVersion("1.0.0")
        .WithInitialization(initialization => initialization
            .WithAction(Initialize))
        .AddEvent<DiceRoll>(e => e
            .AddEffect(effect => effect
                .WithComponent<DiceRoll.Effect>()))
        .AddEvent<Attack>(e => e
            .AddEffect(effect => effect
                .WithComponent<Attack.Effect>()))
        .AddEvent<DealDamage>(e => e
            .AddEffect(effect => effect
                .WithComponent<DealDamage.Effect>()));

    private static void Initialize(IRegistry registry)
    {
        var pipeline = registry.GetRequired<IPipeline>();
        var weapon = new Weapon(new D6(), 5);
        var attacker = new Attacker("Thomas", weapon, 3);
        var defender = new Defender("Lucas", 50, 0);
        pipeline.Raise(new Attack(attacker, defender));
    }
}

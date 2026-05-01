namespace RpgCompanion.DnD;

using Core;

public class Manifest : IManifest
{
    public void Configure(IPluginConfiguration plugin) => plugin
        .WithKey("DND_5E")
        .WithName("D&D 5e")
        .WithVersion("1.0.0")
        .WithInitialization(Initialize)
        .AddEvent<DiceRoll>(e => e
            .WithKey("")
            .AddRule(rule => rule
                .WithKey("")
                .Export(DiceRoll.Apply)))
        .AddEvent<Attack>(e => e
            .WithKey("")
            .AddRule(rule => rule
                .WithKey("")
                .Export<Attack.Definition>()))
        .AddEvent<DealDamage>(e => e
            .WithKey("")
            .AddRule(rule => rule
                .WithKey("")
                .Export(DealDamage.Apply)
                .WithCondition(condition => condition
                    .WithKey("")
                    .Export(DealDamage.ShouldApply))));

    private static void Initialize(IRegistry registry, PluginKey pluginKey)
    {
        var pipeline = registry.GetRequired<ITrigger>();
        var weapon = new Weapon(new Dice.D6(), 5);
        var attacker = new Attacker("Thomas", weapon, 3);
        var defender = new Defender("Lucas", 50, 0);
        pipeline.Raise(new Attack(attacker, defender));
    }
}

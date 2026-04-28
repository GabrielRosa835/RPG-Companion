namespace RpgCompanion.DnD;

using Core;

public class Manifest : IManifest
{
    public void Configure(IPluginConfiguration plugin) => plugin
        .WithName("D&D 5e")
        .WithVersion("1.0.0")
        .WithInitialization(initialization => initialization.Export(Initialize))
        .AddEvent<DiceRoll>(e =>
        {
            e.AddRule(rule => rule.Export(DiceRoll.Apply));
        })
        .AddEvent<Attack>(e =>
        {
            e.AddRule(rule => rule.Export((r, key) => Attack.Apply(r.Get<IPipeline>()!)));
        })
        .AddEvent<DealDamage>(e => e.AddRule(rule =>
        {
            rule.Export(DealDamage.Apply);
            rule.WithCondition(condition => condition.Export(DealDamage.ShouldApply));
        }));

    private static void Initialize(IRegistry registry)
    {
        var pipeline = registry.GetRequired<IPipeline>();
        var weapon = new Weapon(new Dice.D6(), 5);
        var attacker = new Attacker("Thomas", weapon, 3);
        var defender = new Defender("Lucas", 50, 0);
        pipeline.Raise(new Attack(attacker, defender));
    }
}

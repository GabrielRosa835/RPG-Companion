namespace RpgCompanion.DnD;

using Core;

public class Manifest : IManifest
{
    public void Configure(IPluginConfiguration plugin) => plugin
        .WithKey("DND_5E")
        .WithName("D&D 5e")
        .WithVersion("1.0.0")
        .WithInitialization(Initialize)
        .AddEvent<DiceRoll.Event>(e => e
            .WithKey(DiceRoll.Event.Key)
            .AddRule(rule => rule
                .WithKey(DiceRoll.Apply.Key)
                .Export(DiceRoll.Apply.Rule)))
        .AddEvent<Attack.Event>(e => e
            .WithKey(Attack.Event.Key)
            .AddRule(rule => rule
                .WithKey(Attack.Definition.Key)
                .Export<Attack.Definition>()))
        .AddEvent<DealDamage.Event>(e => e
            .WithKey(DealDamage.Event.Key)
            .AddRule(rule => rule
                .WithKey(DealDamage.Apply.Key)
                .Export(DealDamage.Apply.Rule)
                .WithCondition(condition => condition
                    .WithKey(DealDamage.ShouldApply.Key)
                    .Export(DealDamage.ShouldApply.Rule))));

    private static void Initialize(IRegistry registry, PluginKey pluginKey)
    {
        var trigger = registry.GetRequired<ITrigger>();
        var weapon = new Weapon(new Dice.D6(), 5);
        var attacker = new Attacker("Thomas", weapon, 3);
        var defender = new Defender("Lucas", 50, 0);
        trigger.Raise(new Attack.Event(attacker, defender));
    }
}

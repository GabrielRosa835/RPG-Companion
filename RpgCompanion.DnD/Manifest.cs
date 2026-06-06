namespace RpgCompanion.DnD;

using Core;
using Core.Toolbox;

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
                .WithKey(DiceRoll.Rule.Key)
                .Export<DiceRoll.Rule>()))
        .AddEvent<Attack.Event>(e => e
            .WithKey(Attack.Event.Key)
            .AddRule(rule => rule
                .WithKey(Attack.Rule.Key)
                .Export<Attack.Rule>()))
        .AddEvent<DealDamage.Event>(e => e
            .WithKey(DealDamage.Event.Key)
            .AddRule(rule => rule
                .WithKey(DealDamage.Rule.Key)
                .Export<DealDamage.Rule>()
                .WithCondition(condition => condition
                    .WithKey(DealDamage.ShouldApply.Key)
                    .Export<DealDamage.ShouldApply>())))
        .AddRule<DiceRoll.Event, IEvent>(rule => rule
            .WithKey(Attack.DiceRollTransition.Key)
            .Export<Attack.DiceRollTransition>())
        .AddActor<GlobalData>(actor => actor
            .WithName("GlobalData")
            .WithKey(GlobalData.Key)
            .WithLifetime(ActorLifetime.Persistent)
            .WithDescription("Centralized storage of global generic data")
            .Export())
        .AddActor<ContextData>(actor => actor
            .WithName("ContextData")
            .WithKey(ContextData.Key)
            .WithLifetime(ActorLifetime.Temporary)
            .WithDescription("Centralized storage of contextual generic data")
            .Export());

    private static void Initialize(IRegistry registry, PluginKey pluginKey)
    {
        var trigger = registry.GetRequired<ITrigger>();
        var attacker = new Player
        {
            Name = "Thomas",
            AttackModifier = 5,
            DamageModifier = 3,
            Weapon = new Weapon
            {
                DamageDice = new Dice.D6(),
            },
        };
        var defender = new Enemy
        {
            Name = "Lucas",
            AC = 15,
            Health = 50,
        };
        trigger.Raise(new Attack.Event(attacker, defender));
    }
}

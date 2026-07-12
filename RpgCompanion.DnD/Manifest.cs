namespace RpgCompanion.DnD;

using _Old;
using _Old._Actors;
using _Old._Events;
using Events;
using Toolbox;
using Player = _Old._Actors.Player;

public class Manifest : IManifest
{
    public void Configure(IPluginConfiguration plugin) => plugin
        .WithInitialization(Initialize)
        .AddEvent<DiceRoll.Event>(e => e
            .AddRule(rule => rule
                .Export(DiceRoll.Handler)))
        .AddEvent<Attack.Event>(e => e
            .AddRule(rule => rule
                .Export(Attack.Handler)))
        .AddEvent<DealDamage.Event>(e => e
            .AddRule(rule => rule
                .Export(DealDamage.Handler)
                .WithCondition(condition => condition
                    .Export(DealDamage.ShouldApply))))
        .AddActor<GlobalData>(actor => actor
            .WithLifetime(ActorLifetime.Persistent)
            .Export())
        .AddActor<ContextData>(actor => actor
            .WithLifetime(ActorLifetime.Temporary)
            .Export());

    private static void Initialize(RuleContext ctx, PluginKey pluginKey)
    {
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
        ctx.Raise(new Attack.Event(attacker, defender));
    }
}

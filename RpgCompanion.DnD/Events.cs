namespace RpgCompanion.DnD;

using Core;

public record Attack(Attacker Attacker, Defender Defender) : IEvent
{
    public class Effect : IEffect<Attack>
    {
        public void Apply(Attack e, IContext context)
        {
            context.Snapshot;
            context.Registry;
            context.Data;
            Console.WriteLine($"Realizando efeito de ataque: \n   Atacante: {e.Attacker.Name} \n   Defensor: {e.Defender.Name}");
            // pipeline
            //     .Raise(new DiceRoll(e.Attacker.Weapon!.DamageDice, e.Attacker.AttackModifier))
            //     .FollowedBy(roll => new DealDamage(e.Defender, roll.Result));
        }
    }
}

public record TryHitWeapon(int Modifier, int TargetValue) : IEvent
{
    public bool Hit { get; private set; }

    public class Effect : IEffect<TryHitWeapon>
    {
        public bool ShouldApply(TryHitWeapon e) => true;
        public void Apply(TryHitWeapon e, IPipeline pipeline)
        {
            Console.WriteLine($"Realizando efeito de tentativa de ataque: Target = {e.TargetValue}, Modifier = {e.Modifier}");
            int result = new D20().Roll() + e.Modifier;
            Console.WriteLine($"Rolagem realizada com resultado: {result}");
            var hit = result >= e.TargetValue;
            e.Hit = hit;
            Console.WriteLine($"Sucesso: {hit}");
        }
    }
}

public record DealDamage(Defender Defender, int Damage) : IEvent
{
    public class Effect : IEffect<DealDamage>
    {
        public bool ShouldApply(DealDamage e) => e.Damage > 0;
        public void Apply(DealDamage e, IPipeline pipeline)
        {
            Console.WriteLine($"Realizando efeito de dano: \n   Defensor: {e.Defender.Name} ({e.Defender.Health}HP) \n   Dano: {e.Damage}");
            e.Defender.Health -= e.Damage;
            if (e.Defender.Health <= 0)
            {
                e.Defender.Health = 0;
            }
            Console.WriteLine($"Vida após dano: {e.Defender.Health}HP");
        }
    }
}

public record DiceRoll(Dice Dice, int Modifier) : IEvent
{
    public int Result { get; private set; }

    public class Effect : IEffect<DiceRoll>
    {
        public bool ShouldApply(DiceRoll e) => true;
        public void Apply(DiceRoll e, IPipeline pipeline)
        {
            Console.WriteLine($"Realizando efeito de rolagem: \n   Dado: {e.Dice}");
            e.Result = e.Dice.Roll() + e.Modifier;
            Console.WriteLine($"Rolagem realizada com resultado: {e.Result}");
        }
    }
}

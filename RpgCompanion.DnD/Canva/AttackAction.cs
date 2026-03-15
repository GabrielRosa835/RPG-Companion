namespace RpgCompanion.DnD;

using RpgCompanion.Core.Contexts;
using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Events;
using RpgCompanion.Core.Events.Producers;
using RpgCompanion.DnD.Canva;

public record PlayerInputEvent : IEvent;

public record AttackAction(Attacker Attacker) : IEvent
{
    public class Rule : IRule<PlayerInputEvent>
    {
        public IEvent Apply(IContext context)
        {
            throw new NotImplementedException();
        }

        public bool ShouldApply(IContext context)
        {
            throw new NotImplementedException();
        }
    }
    public class Effect : IEffect<AttackAction>
    {
        public void Apply(AttackAction @event, IPipeline context)
        {
            throw new NotImplementedException();
        }

        public bool ShouldApply(AttackAction @event)
        {
            throw new NotImplementedException();
        }
    }
}

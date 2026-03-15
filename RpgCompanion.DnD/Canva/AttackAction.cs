namespace RpgCompanion.DnD;

using RpgCompanion.Core.Events;
using RpgCompanion.DnD.Canva;

public record PlayerInputEvent : IEvent;

public record AttackAction(Attacker Attacker) : IEvent
{

}

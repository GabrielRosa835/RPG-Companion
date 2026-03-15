using RpgCompanion.Core.Events;

namespace RpgCompanion.DnD.Canva;

internal record MovementAction(object Runner, (int X, int Y) StartingPosition, (int X, int Y) EndingPosition) : IEvent
{
}

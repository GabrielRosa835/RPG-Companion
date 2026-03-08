using RpgCompanion.Core.Engine;

namespace RpgCompanion.Core.Contexts;

/// <summary>
/// Readonly context for Rules
/// </summary>
public interface IContextSnapshot
{
   IReadonlyContextData Data { get; }
   IRegistry Registry { get; }
}

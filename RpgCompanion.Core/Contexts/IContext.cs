using RpgCompanion.Core.Engine;

namespace RpgCompanion.Core.Contexts;

/// <summary>
/// Readonly context for Rules
/// </summary>
public interface IContext
{
   IContextData Data { get; }
   IRegistry Registry { get; }
}

using RpgCompanion.Core.Engine;

namespace RpgCompanion.Core.Meta;

using Engine;

public interface IInitializer
{
   void Initialize (IRegistry registry);
}

public delegate void InitializerAction (IRegistry registry);

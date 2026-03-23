using RpgCompanion.Core.Engine;

namespace RpgCompanion.Core.Meta;

public interface IInitializer
{
   void Initialize (IRegistry registry);
}

public delegate void InitializerAction (IRegistry registry);

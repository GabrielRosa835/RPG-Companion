using RpgCompanion.Core.Engine;

namespace RpgCompanion.Core.Meta;

public interface IInitializer<TSystem>  where TSystem : ISystem
{
   void Initialize (IRegistry registry);
}

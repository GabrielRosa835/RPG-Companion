using RpgCompanion.Core.Engine;

namespace RpgCompanion.Core.Meta;

public interface IInitializer<TSystem>  where TSystem : IPlugin
{
   void Initialize (IRegistry registry);
}

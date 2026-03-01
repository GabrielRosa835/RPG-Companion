using RpgCompanion.Core.Engine;

namespace RpgCompanion.Core.Meta;

public interface IManifest<TSystem> where TSystem : ISystem
{
   void RegisterInitializer(IRegistryCollection services);
   void RegisterComponents(IRegistryCollection services);
}

using RpgCompanion.Core.Engine;

namespace RpgCompanion.Core.Meta;

public interface IManifest<out TSystem> where TSystem : ISystem
{
   Type Initializer { get; }
   void Setup(IRegistryCollection services);
}

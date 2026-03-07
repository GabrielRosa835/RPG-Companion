using RpgCompanion.Core.Engine;

namespace RpgCompanion.Core.Meta;

public interface IManifest<out TSystem> where TSystem : IPlugin
{
   Type Initializer { get; }
   void Setup(IPluginBuilder builder);
}

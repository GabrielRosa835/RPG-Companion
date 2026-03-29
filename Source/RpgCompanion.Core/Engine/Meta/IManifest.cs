using RpgCompanion.Core.Engine;

namespace RpgCompanion.Core.Meta;

using Engine;

public interface IManifest
{
   void Configure(IPluginBuilder builder);
}

using RpgCompanion.Core.Engine;
using RpgCompanion.Core.Meta;

namespace RpgCompanion.DnD.Meta;

internal class DnD5eInitializer : IInitializer<DnD5e>
{
   public void Initialize (IRegistry registry)
   {

      Console.WriteLine(nameof(DnD5eInitializer));

   }
}

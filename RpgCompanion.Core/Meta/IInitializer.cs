using RpgCompanion.Core.Engine;

namespace RpgCompanion.Core.Meta;


public interface IInitializer
{
   void Initialize (IRegistry registry);
}
public interface IInitializer<TSystem> : IInitializer where TSystem : ISystem
{
}

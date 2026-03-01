namespace RpgCompanion.Core.Engine;

public interface IRegistryCollection
{
   void Add<TImplementation> () where TImplementation : class;
   void Add<TInterface, TImplementation> ()
      where TInterface : class
      where TImplementation : class, TInterface;
}

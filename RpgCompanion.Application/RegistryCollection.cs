using Microsoft.Extensions.DependencyInjection;

using RpgCompanion.Core.Engine;

namespace RpgCompanion.Application;

internal class RegistryCollection (IServiceCollection services) : IRegistryCollection
{
   public void Add<TImplementation> () where TImplementation : class
   {
      services.AddTransient<TImplementation> ();
   }

   public void Add<TInterface, TImplementation> ()
      where TInterface : class
      where TImplementation : class, TInterface
   {
      services.AddTransient<TInterface, TImplementation>();
   }
}

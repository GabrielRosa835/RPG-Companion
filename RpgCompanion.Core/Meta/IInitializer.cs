using RpgCompanion.Core.Engine;

namespace RpgCompanion.Core.Meta;

public interface IInitializer
{
   void Initialize (IRegistry registry);
}

public record EmptyInitializer() : IInitializer
{
   public void Initialize(IRegistry registry)
   {
      Console.WriteLine($"{nameof(EmptyInitializer)} initialized");
   }
}
namespace RpgCompanion.Core;

public interface IInitializer
{
   void Initialize (IRegistry registry);
}

public delegate void InitializerAction (IRegistry registry);

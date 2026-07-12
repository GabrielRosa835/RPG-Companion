namespace RpgCompanion.DnD;

using Events;
using Toolbox;

public interface IEnvironment;

public interface IClock
{
    DateTime Now { get; }
}

public interface IBiome;

public interface ICulture;

public static class Canva
{
    public static readonly StorageKey<IEnvironment> EnvironmentKey = new(nameof(EnvironmentKey));

    extension(World world)
    {
        public IEnvironment? Environment => world.GetOrDefault(EnvironmentKey);
    }

    public static void Teste(World world)
    {

    }
}

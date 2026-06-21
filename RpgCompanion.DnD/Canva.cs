namespace RpgCompanion.DnD;

using Core;
using Core.Toolbox;

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
        public IEnvironment? Environment => world.Variables.GetOrDefault(EnvironmentKey);
    }

    public static void Teste(World world)
    {

    }
}

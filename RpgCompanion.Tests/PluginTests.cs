namespace RpgCompanion.Tests;

using System.Reflection;

using RpgCompanion.Application;

public class PluginTests
{
    private const string? AppFolder = null;
    private readonly PluginManager _manager;

    public PluginTests() => _manager = new PluginManager();

    [Fact]
    public void ShouldFindPluginsWithoutErrors()
    {
        string pluginsFolder = AppFolder ?? Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "plugins");
        Console.WriteLine(pluginsFolder);

        var descriptors = _manager.FindPlugins(pluginsFolder);

        Assert.Equal(_manager.Descriptors.ToHashSet(), descriptors.ToHashSet());
        Assert.True(_manager.Descriptors.Count > 0);
        Assert.Contains(_manager.Descriptors, d => d.Resource.Contains("RpgCompanion.DnD"));
    }
}

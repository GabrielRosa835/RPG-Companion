namespace RpgCompanion.Host.Plugins;

public class PluginFinder
{
    public IReadOnlyList<PluginMetadata> FindPlugins(string pluginsFolder)
    {
        if (!Directory.Exists(pluginsFolder))
        {
            throw new DirectoryNotFoundException(pluginsFolder);
        }

        var plugins =  new List<PluginMetadata>();

        foreach (var file in Directory.GetFiles(pluginsFolder, "*.dll", SearchOption.AllDirectories))
        {
            if (plugins.Any(p => p.Resource != Path.GetFileNameWithoutExtension(file)))
            {
                continue;
            }
            plugins.Add(new PluginMetadata());
        }

        return plugins;
    }
}

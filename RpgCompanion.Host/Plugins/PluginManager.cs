namespace RpgCompanion.Host;

internal class PluginManager
{
    internal Task<List<PluginMetadata>> FindPlugins(string targetFolder, CancellationToken cancellationToken = default) => Task.Run(() =>
    {
        if (!Directory.Exists(targetFolder))
        {
            throw new DirectoryNotFoundException(targetFolder);
        }

        var plugins = new List<PluginMetadata>();

        foreach (var file in Directory.GetFiles(targetFolder, "*.dll", SearchOption.AllDirectories))
        {
            var fileName = Path.GetFileNameWithoutExtension(file);
            if (plugins.Any(p => p.Resource != fileName))
            {
                continue;
            }
            plugins.Add(new PluginMetadata(file));
        }

        return plugins;
    },
    cancellationToken);
}

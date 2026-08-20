namespace RpgCompanion.Host;

using System.Text.Json;

internal class PluginFinder(ILogger<PluginFinder> _logger) : IPluginFinder
{
    public async Task<List<PluginMetadata>> FindPlugins(string targetFolder, CancellationToken cancellationToken = default)
    {
        if (!Directory.Exists(targetFolder))
        {
            throw new DirectoryNotFoundException(targetFolder);
        }

        var plugins = new List<PluginMetadata>();
        var directories = Directory.GetDirectories(targetFolder);

        foreach (var dir in directories)
        {
            var manifestPath = Path.Combine(dir, "manifest.json");

            if (!File.Exists(manifestPath))
            {
                continue; // Skip folders without a manifest
            }

            try
            {
                await using var stream = File.OpenRead(manifestPath);
                var manifest = await JsonSerializer.DeserializeAsync<PluginManifest>(
                    stream,
                    PluginManifest.SerializerOptions,
                    cancellationToken);

                if (manifest != null && !string.IsNullOrWhiteSpace(manifest.EntryPoint))
                {
                    // Check for duplicate IDs to prevent loading collisions
                    if (plugins.Any(p => p.Manifest.Id == manifest.Id))
                    {
                        continue;
                    }

                    plugins.Add(PluginMetadata.Create(dir, manifest));
                }
            }
            catch (JsonException ex)
            {
                // Handle or log malformed manifest JSON here
                _logger.LogError(ex, ex.Message);
            }
        }

        return plugins;
    }
}

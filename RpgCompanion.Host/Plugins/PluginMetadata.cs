namespace RpgCompanion.Host;

using System.Reflection;

internal class PluginMetadata
{
    internal string FolderPath { get; init; }
    internal PluginManifest Manifest { get; init; }
    internal string EntryPointPath => Path.Combine(FolderPath, "bin", Manifest.EntryPoint); // The absolute path to the main dll inside the bin folder

    internal bool Loaded { get; set; }
    internal PluginLoadContext LoadContext { get; set; } = default!;
    internal Assembly Assembly { get; set; } = default!;

    internal bool Initialized { get; set; }
    internal PluginDescriptor Descriptor { get; set; } = default!;
    internal IServiceProvider Services { get; set; } = default!;

    internal PluginMetadata(string folder, PluginManifest manifest)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(folder);
        ArgumentNullException.ThrowIfNull(manifest);

        if (!Directory.Exists(folder))
        {
            throw new DirectoryNotFoundException(folder);
        }
        if (!Path.IsPathRooted(folder))
        {
            throw new InvalidOperationException($"Plugin folder path must be rooted to ensure unambiguous location.");
        }

        FolderPath = folder;
        Manifest = manifest;
    }
}

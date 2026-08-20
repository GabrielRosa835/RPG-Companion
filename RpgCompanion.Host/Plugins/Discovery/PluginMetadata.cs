namespace RpgCompanion.Host;

internal class PluginMetadata
{
    internal Guid Id { get; } = Guid.CreateVersion7();
    internal string FolderPath { get; private set; } = default!;
    internal PluginManifest Manifest { get; private set; } = default!;
    internal string EntryPointPath => BuildEntryPointPath(FolderPath, Manifest);

    private static string BuildEntryPointPath(string folderPath, PluginManifest manifest)
    {
        // The absolute path to the main dll inside the bin folder
        return Path.Combine(folderPath, "bin", manifest.EntryPoint);
    }

    private PluginMetadata()
    {
    }

    protected PluginMetadata(PluginMetadata metadata)
    {
        Id = metadata.Id;
        FolderPath = metadata.FolderPath;
        Manifest = metadata.Manifest;
    }

    internal static PluginMetadata Create(string folder, PluginManifest manifest)
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

        string entryPointPath = BuildEntryPointPath(folder, manifest);
        if (!File.Exists(entryPointPath))
        {
            throw new FileNotFoundException($"Plugin entry point not found at {entryPointPath}");
        }

        return new PluginMetadata
        {
            FolderPath = folder,
            Manifest = manifest,
        };
    }
}

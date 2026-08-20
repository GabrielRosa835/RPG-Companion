namespace RpgCompanion.Host;

using System.Text.Json;

// TODO: Validation across manifest.json fields
internal class PluginManifest
{
    public string Id { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Version { get; init; } = string.Empty;
    public string Author { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string EntryPoint { get; init; } = string.Empty;
    public string PdkVersion { get; init; } = string.Empty;

    internal static JsonSerializerOptions SerializerOptions { get; } = new()
    {
        // Makes mapping forgiving if someone types "entry-point" or "Entry-Point"
        PropertyNameCaseInsensitive = true,

        // Maps "pdk-version" in JSON to "PdkVersion" in C#
        PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower,

        // Prevents crashes from leftover commas
        AllowTrailingCommas = true,

        // Allows developers to document their manifests with // comments
        ReadCommentHandling = JsonCommentHandling.Skip
    };
}

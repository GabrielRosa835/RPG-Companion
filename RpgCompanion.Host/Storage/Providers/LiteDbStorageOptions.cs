namespace RpgCompanion.Host;

internal class LiteDbStorageOptions
{
    private const string Filename = "storage.db";

    public bool Shared { get; set; } = false;
    public bool InMemory { get; set; } = false;
    public string PluginFolder { get; set; } = string.Empty;
    public string ConnectionString => BuildConnectionString();
    private string FilePath => Path.Combine(PluginFolder, "data", Filename);

    /// Connection String Examples:
    /// - Basic local file: "Filename=PluginData.db;"
    /// - Shared mode (allows concurrent read/write from different processes, great for debugging): "Filename=PluginData.db;Connection=Shared;"
    /// - In-Memory (useful for testing): "Filename=:memory:;"
    private string BuildConnectionString()
    {
        string filename = "Filename=" + (InMemory ? ":memory:" : FilePath) + ";";
        string connection = Shared ? "Connection=Shared;" : "";
        return connection + filename + connection;
    }
}

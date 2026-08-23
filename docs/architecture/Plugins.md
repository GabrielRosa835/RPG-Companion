# Plugins

**Plugins** are external extensions loaded into the Host (`RpgCompanion.Host`) to enhance its services (e.g., `RpgCompanion.DnD`, `RpgCompanion.Toolbox`).

## Lifecycle
1. **Discovery:** The system locates the plugin `.dll` and adds it to the managed list.
2. **Loading:** The assembly is scanned to find the `IManifest` implementation (leveraging `AssemblyLoadContext`).
3. **Initialization:** Custom plugin snippets execute after manifest configurations are applied and the plugin's `ServiceProvider` is built.

## Services and Configuration
- Each plugin has its own `ServiceProvider`, wrapped as a `Registry` in the PDK.
- Plugins expose a public class with a parameterless constructor implementing `IManifest`.
- Core components are explicitly defined via `IPluginConfiguration` using a fluent builder pattern.
